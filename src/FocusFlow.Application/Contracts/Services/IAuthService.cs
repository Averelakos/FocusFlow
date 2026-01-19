public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto registerDto, CancellationToken ct);
    Task<string> LoginAsync(LoginDto loginDto, CancellationToken ct);
}