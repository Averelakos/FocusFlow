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
    
    public async Task<string> RegisterAsync(RegisterDto registerDto, CancellationToken ct)
    {
       if (await UserExistsAsync(registerDto.Username, registerDto.Email, ct))
       {
            return string.Empty;
       }

       User newUser = registerDto.ToEntity();
       var addedUser = await _userRepository.AddAsync(newUser, ct);
       return _tokenProviderService.GenerateAccessToken(addedUser);
    }

    public async Task<string> LoginAsync(LoginDto loginDto, CancellationToken ct)
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
            return string.Empty;
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return string.Empty;
        }
        
        return _tokenProviderService.GenerateAccessToken(user);
    }


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