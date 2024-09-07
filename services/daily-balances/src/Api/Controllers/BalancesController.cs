using DailyBalances.Core.Queries.GetDailyBalancesQuery;
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
    public async Task<IActionResult> RegisterDebit(DateTime day)
    {
        var d = day.Date;
        var query = new GetDailyBalancesQuery
        {
            MerchantId = UserId,
            Day = d.Kind == DateTimeKind.Utc ? d : DateTime.SpecifyKind(d, DateTimeKind.Utc),
        };

        var output = await sender.Send(query);

        return Ok(output);
    }
}
