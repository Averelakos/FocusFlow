using Microsoft.AspNetCore.Mvc;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger) 
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto request, CancellationToken ct)
    {
        // Registration logic here
        var response = await _authService.RegisterAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto request, CancellationToken ct)
    {
        // Authenticate user
        var response = await _authService.LoginAsync(request, ct);
        return Ok(response);
    }
    
}