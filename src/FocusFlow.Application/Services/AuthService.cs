using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProviderService _tokenProviderService;

    public AuthService(IUserRepository userRepository, ITokenProviderService tokenProviderService)
    {
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
    }
    
    /// <summary>
    /// Registers a new user with username, email, and hashed password
    /// </summary>
    /// <param name="registerDto">The registration data containing user details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with success status and JWT token if successful</returns>
    public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto, CancellationToken ct)
    {
       if (await UserExistsAsync(registerDto.Username, registerDto.Email, ct))
       {
            return new AuthResponse { Success = false, Message = "User already exists." };
       }

       User newUser = registerDto.ToEntity();
       var addedUser = await _userRepository.AddAsync(newUser, ct);
       return new AuthResponse { Success = true, Token = _tokenProviderService.GenerateAccessToken(addedUser) };
    }

    /// <summary>
    /// Authenticates a user with username/email and password
    /// </summary>
    /// <param name="loginDto">The login credentials (username or email and password)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with success status and JWT token if credentials are valid</returns>
    public async Task<AuthResponse> LoginAsync(LoginDto loginDto, CancellationToken ct)
    {
        bool isEmail = loginDto.UsernameOrEmail.IsEmail();

        User? user;

        if (isEmail)
        {
            user = await _userRepository.GetByParameterAsync(u => u.Email == loginDto.UsernameOrEmail, ct);
        }
        else
        {
            user = await _userRepository.GetByParameterAsync(u => u.Username == loginDto.UsernameOrEmail, ct);
        }

        if (user is null)
        {
            return new AuthResponse { Success = false, Message = "User not found." };
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return new AuthResponse { Success = false, Message = "Invalid password." };
        }
        
        return new AuthResponse { Success = true, Token = _tokenProviderService.GenerateAccessToken(user) };
    }

    /// <summary>
    /// Checks if a user with the given username or email already exists
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <param name="email">The email to check</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if a user with the username or email exists</returns>
    public async Task<bool> UserExistsAsync(string? username, string? email, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(email))
        {
            var emailExists = await _userRepository.AnyAsync(u => u.Email == email, ct);
            return emailExists;
        }
        else
        {
            var usernameExists = await _userRepository.AnyAsync(u => u.Username == username, ct);
            return usernameExists;
        }
    }
}