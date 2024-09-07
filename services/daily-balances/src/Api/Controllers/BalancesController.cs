using DailyBalances.Core.Payloads.GetDailyBalances;
using DailyBalances.Core.Queries.GetDailyBalances;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyBalances.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/balances")]
public sealed class BalancesController : CustomControllerBase
{
    private readonly ISender sender;

    public BalancesController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetDailyBalancesQueryOutput), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> GetBalance([FromQuery] GetDailyBalancesPayload payload)
    {
        var query = payload.AsGetDailyBalancesQuery(UserId);

        var output = await sender.Send(query);

        return Ok(output);
    }
}
