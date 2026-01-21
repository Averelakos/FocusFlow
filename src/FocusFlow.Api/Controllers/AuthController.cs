using Microsoft.AspNetCore.Mvc;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger) 
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user with username, email, and password
    /// </summary>
    /// <param name="request">The registration data containing user details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with success status and JWT token if registration is successful</returns>
    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto request, CancellationToken ct)
    {
        // Registration logic here
        var response = await _authService.RegisterAsync(request, ct);
        return Ok(response);
    }

    /// <summary>
    /// Authenticates a user with username/email and password
    /// </summary>
    /// <param name="request">The login credentials (username or email and password)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with success status and JWT token if credentials are valid</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto request, CancellationToken ct)
    {
        // Authenticate user
        var response = await _authService.LoginAsync(request, ct);
        return Ok(response);
    }
    
}