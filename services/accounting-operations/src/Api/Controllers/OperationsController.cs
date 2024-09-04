using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingOperations.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/operations")]
public sealed class OperationsController : CustomControllerBase
{
    private readonly ILogger<OperationsController> logger;

    public OperationsController(ILogger<OperationsController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok();
    }
}
