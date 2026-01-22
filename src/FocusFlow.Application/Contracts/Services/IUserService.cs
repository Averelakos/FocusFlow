using FocusFlow.Application.Dtos.Auth;

public interface IUserService
{
    /// <summary>
    /// Gets a lightweight list of users for dropdowns
    /// </summary>
    IEnumerable<UserLookupDto> GetUsersLookup();
}
