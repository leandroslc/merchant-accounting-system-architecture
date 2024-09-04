using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AccountingOperations.Api.Controllers;

public abstract class CustomControllerBase : ControllerBase
{
    protected string UserId => User.FindFirstValue("sub") ?? string.Empty;
}
