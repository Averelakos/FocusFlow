using Microsoft.AspNetCore.Mvc;
using FocusFlow.Application.Dtos.Auth;

/// <summary>
/// Controller for managing users
/// </summary>
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets a lightweight list of users for dropdowns (ID and FullName only)
    /// </summary>
    [HttpGet("Lookup")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<UserLookupDto>>> GetLookup()
    {
        var users = _userService.GetUsersLookup();
        return Ok(users);
    }
}
