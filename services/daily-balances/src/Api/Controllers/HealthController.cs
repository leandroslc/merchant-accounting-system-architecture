using Microsoft.AspNetCore.Mvc;

namespace DailyBalances.Api.Controllers;

[ApiController]
[Route("")]
public sealed class HealthController : CustomControllerBase
{
    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok();
    }
}
