using Microsoft.AspNetCore.Mvc;

public class ProjectTaskController : BaseApiController
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(ILogger<ProjectTaskController> logger, IProjectTaskService projectTaskService) : base(logger) 
    {
        _projectTaskService = projectTaskService;
    }

    [HttpGet("Getall")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectTaskSimpleDto>>> GetAll(
        [FromQuery] long? projectId = null,
        [FromQuery] ProjectTaskStatus? status = null,
        [FromQuery] ProjectTaskPriority? priority = null)
    {
        var projectTasks = _projectTaskService.GetAll(projectId, status, priority);
        return Ok(projectTasks);
    }

    [HttpGet("Statistics")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskStatisticsDto>> GetStatistics()
    {
        var statistics = _projectTaskService.GetStatistics();
        return Ok(statistics);
    }

    [HttpGet("Statistics/ByProject")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectStatisticsDto>>> GetProjectStatistics()
    {
        var statistics = _projectTaskService.GetProjectStatistics();
        return Ok(statistics);
    }

    [HttpGet("Project/{projectId}")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectTaskSimpleDto>>> GetByProjectId(long projectId)
    {
        var projectTasks = _projectTaskService.GetByProjectId(projectId);
        return Ok(projectTasks);
    }

    [HttpGet("{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> GetById(long id, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.GetProjectTaskById(id, ct);
        return Ok(projectTask);
    }

    [HttpPost("Create")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> Create([FromBody] CreateProjectTaskDto request, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = projectTask.Id }, projectTask);
    }

    [HttpPut("Update")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> Update([FromBody] UpdateProjectTaskDto request, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.UpdateAsync(request, ct);
        return Ok(projectTask);
    }

    [HttpDelete("Delete/{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult> Delete(long id, CancellationToken ct)
    {
        await _projectTaskService.DeleteAsync(id, ct);
        return NoContent();
    }
    
}