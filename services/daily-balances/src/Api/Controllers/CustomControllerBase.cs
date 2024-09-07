using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DailyBalances.Api.Controllers;

public abstract class CustomControllerBase : ControllerBase
{
    protected string UserId => User.FindFirstValue("sub") ?? string.Empty;
}
