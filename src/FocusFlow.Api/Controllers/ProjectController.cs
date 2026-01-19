using Microsoft.AspNetCore.Mvc;

public class ProjectController : BaseApiController
{
    private readonly IAuthService _authService;

    public ProjectController(ILogger<ProjectController> logger, IAuthService authService) : base(logger) 
    {
        _authService = authService;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        // Registration logic here
        
        return Ok(new { token = "test" });
    }
    
}