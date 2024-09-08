using Microsoft.AspNetCore.Mvc;

namespace SimpleAuth.Api.Controllers;

[ApiController]
[Route("")]
public sealed class HealthController : ControllerBase
{
    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok();
    }
}
