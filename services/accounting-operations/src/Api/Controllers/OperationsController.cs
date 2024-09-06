using AccountingOperations.Core.Payloads.OperationRegistration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingOperations.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/operations")]
public sealed class OperationsController : CustomControllerBase
{
    private readonly ISender sender;

    public OperationsController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost("debit")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegisterDebit(RegisterOperationPayload payload)
    {
        var command = payload.AsDebitRegistrationCommand(UserId);

        await sender.Send(command);

        return NoContent();
    }

    [HttpPost("credit")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegisterCredit(RegisterOperationPayload payload)
    {
        var command = payload.AsCreditRegistrationCommand(UserId);

        await sender.Send(command);

        return NoContent();
    }
}
