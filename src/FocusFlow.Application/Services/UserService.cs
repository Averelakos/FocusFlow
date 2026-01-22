using FocusFlow.Application.Dtos.Auth;

/// <summary>
/// Service for managing users
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Gets a lightweight list of users for dropdowns
    /// </summary>
    public IEnumerable<UserLookupDto> GetUsersLookup()
    {
        return _userRepository
            .Queryable()
            .Select(u => new UserLookupDto
            {
                Id = u.Id,
                FullName = u.FullName
            })
            .ToList();
    }
}
