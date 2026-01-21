public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterDto registerDto, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginDto loginDto, CancellationToken ct);
}