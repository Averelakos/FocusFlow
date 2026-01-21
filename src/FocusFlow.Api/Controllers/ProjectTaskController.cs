using Microsoft.AspNetCore.Mvc;

public class ProjectTaskController : BaseApiController
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(ILogger<ProjectTaskController> logger, IProjectTaskService projectTaskService) : base(logger) 
    {
        _projectTaskService = projectTaskService;
    }

    /// <summary>
    /// Gets all project tasks with optional filtering
    /// </summary>
    /// <param name="projectId">Optional project ID to filter by</param>
    /// <param name="status">Optional status to filter by (Todo, InProgress, Done)</param>
    /// <param name="priority">Optional priority to filter by (Low, Medium, High, Critical)</param>
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

    /// <summary>
    /// Gets overall task statistics (total, completed, overdue, in progress, to-do)
    /// </summary>
    [HttpGet("Statistics")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskStatisticsDto>> GetStatistics()
    {
        var statistics = _projectTaskService.GetStatistics();
        return Ok(statistics);
    }

    /// <summary>
    /// Gets task statistics grouped by project
    /// </summary>
    [HttpGet("Statistics/ByProject")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectStatisticsDto>>> GetProjectStatistics()
    {
        var statistics = _projectTaskService.GetProjectStatistics();
        return Ok(statistics);
    }

    /// <summary>
    /// Gets all tasks for a specific project
    /// </summary>
    [HttpGet("Project/{projectId}")]
    [AuthorizeJwt]
    public async Task<ActionResult<List<ProjectTaskSimpleDto>>> GetByProjectId(long projectId)
    {
        var projectTasks = _projectTaskService.GetByProjectId(projectId);
        return Ok(projectTasks);
    }

    /// <summary>
    /// Gets a project task by its ID with full details
    /// </summary>
    [HttpGet("{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> GetById(long id, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.GetProjectTaskById(id, ct);
        return Ok(projectTask);
    }

    /// <summary>
    /// Creates a new project task and broadcasts notification via SignalR
    /// </summary>
    [HttpPost("Create")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> Create([FromBody] CreateProjectTaskDto request, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = projectTask.Id }, projectTask);
    }

    /// <summary>
    /// Updates an existing project task and broadcasts notification via SignalR
    /// </summary>
    [HttpPut("Update")]
    [AuthorizeJwt]
    public async Task<ActionResult<ProjectTaskDetailDto>> Update([FromBody] UpdateProjectTaskDto request, CancellationToken ct)
    {
        var projectTask = await _projectTaskService.UpdateAsync(request, ct);
        return Ok(projectTask);
    }

    /// <summary>
    /// Deletes a project task by its ID and broadcasts notification via SignalR
    /// </summary>
    [HttpDelete("Delete/{id}")]
    [AuthorizeJwt]
    public async Task<ActionResult> Delete(long id, CancellationToken ct)
    {
        await _projectTaskService.DeleteAsync(id, ct);
        return NoContent();
    }
    
}