using Microsoft.AspNetCore.Mvc;

public class ProjectController : BaseApiController
{
    private readonly IAuthService _authService;

    public ProjectController(ILogger<ProjectController> logger, IAuthService authService) : base(logger) 
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request, CancellationToken ct)
    {
        // Registration logic here
        var token = await _authService.RegisterAsync(request, ct);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request, CancellationToken ct)
    {
        // Authenticate user
        var token = await _authService.LoginAsync(request, ct);
        return Ok(new { token });
    }
    
}