using Microsoft.AspNetCore.Mvc;

public class ProjectController : BaseApiController
{
    private readonly IProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger, IProjectService projectService) : base(logger) 
    {
        _projectService = projectService;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        // Registration logic here
        
        return Ok(new { token = "test" });
    }

    [HttpGet("Getall")]
    public async Task<ActionResult<List<ProjectSimpleDto>>> GetAll()
    {
        // Registration logic here
        var projects =  _projectService.GetAll();
        return Ok(projects);
    }
    
}