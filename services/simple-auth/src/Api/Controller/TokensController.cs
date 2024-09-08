using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleAuth.Api.Models;

namespace SimpleAuth.Api.Controllers;

[ApiController]
[Route("v1/tokens")]
public sealed class TokensController : ControllerBase
{
    private readonly AuthorityOptions options;
    private readonly IHttpClientFactory httpClientFactory;

    public TokensController(
        AuthorityOptions options,
        IHttpClientFactory httpClientFactory)
    {
        this.options = options;
        this.httpClientFactory = httpClientFactory;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<string> GetToken([FromQuery] string? userId)
    {
        var client = httpClientFactory.CreateClient();

        var response = await client.GetAsync(options.Url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<AuthorityResponse>()
            ?? throw new InvalidOperationException("Content not found");

        var data = content.Data.FirstOrDefault()
            ?? throw new InvalidOperationException("Data is empty");

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(new()
        {
            Issuer = data.Key,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
                SecurityAlgorithms.HmacSha256Signature),
            Claims = new Dictionary<string, object>
            {
                ["sub"] = userId ?? Guid.NewGuid().ToString(),
            },
        });

        return tokenHandler.WriteToken(token);
    }
}
