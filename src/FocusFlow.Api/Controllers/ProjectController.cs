using Microsoft.AspNetCore.Mvc;

public class ProjectController : BaseApiController
{
    private readonly IProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger, IProjectService projectService) : base(logger) 
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Gets all projects for the current user
    /// </summary>
    [HttpGet("Getall")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectSimpleDto>>> GetAll()
    {
        // Registration logic here
        var projects =  _projectService.GetAll();
        return Ok(projects);
    }

    /// <summary>
    /// Gets a lightweight list of projects for dropdowns (ID and Name only, cached for 24 hours)
    /// </summary>
    [HttpGet("Lookup")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectLookupDto>>> GetLookup()
    {
        var projects = _projectService.GetLookup();
        return Ok(projects);
    }

    /// <summary>
    /// Gets a project by its ID with full details
    /// </summary>
    [HttpGet("{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> GetById(long id, CancellationToken ct)
    {
        var project = await _projectService.GetProjectById(id, ct);
        return Ok(project);
    }

    /// <summary>
    /// Creates a new project and invalidates the lookup cache
    /// </summary>
    [HttpPost("Create")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> Create([FromBody] CreateProjectDto request, CancellationToken ct)
    {
        var project = await _projectService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = project.Id }, project);
    }

    /// <summary>
    /// Updates an existing project and invalidates the lookup cache
    /// </summary>
    [HttpPut("Update")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> Update([FromBody] UpdateProjectDto request, CancellationToken ct)
    {
        var project = await _projectService.UpdateAsync(request, ct);
        return Ok(project);
    }

    /// <summary>
    /// Deletes a project by its ID and invalidates the lookup cache
    /// </summary>
    [HttpDelete("Delete/{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult> Delete(long id, CancellationToken ct)
    {
        await _projectService.DeleteAsync(id, ct);
        return NoContent();
    }
    
}