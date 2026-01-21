using Microsoft.AspNetCore.Mvc;

public class ProjectController : BaseApiController
{
    private readonly IProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger, IProjectService projectService) : base(logger) 
    {
        _projectService = projectService;
    }

    [HttpGet("Getall")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectSimpleDto>>> GetAll()
    {
        // Registration logic here
        var projects =  _projectService.GetAll();
        return Ok(projects);
    }

    [HttpGet("Lookup")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectLookupDto>>> GetLookup()
    {
        var projects = _projectService.GetLookup();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> GetById(long id, CancellationToken ct)
    {
        var project = await _projectService.GetProjectById(id, ct);
        return Ok(project);
    }

    [HttpPost("Create")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> Create([FromBody] CreateProjectDto request, CancellationToken ct)
    {
        var project = await _projectService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = project.Id }, project);
    }

    [HttpPut("Update")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectDetailDto>> Update([FromBody] UpdateProjectDto request, CancellationToken ct)
    {
        var project = await _projectService.UpdateAsync(request, ct);
        return Ok(project);
    }

    [HttpDelete("Delete/{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult> Delete(long id, CancellationToken ct)
    {
        await _projectService.DeleteAsync(id, ct);
        return NoContent();
    }
    
}