public interface IProjectService
{
    Task<ProjectDetailDto> GetProjectById(long id, CancellationToken ct);
    IEnumerable<ProjectSimpleDto> GetAll();
    IEnumerable<ProjectLookupDto> GetLookup();
    Task<ProjectDetailDto> CreateAsync(CreateProjectDto request, CancellationToken ct);
    Task<ProjectDetailDto> UpdateAsync(UpdateProjectDto request, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
}

public interface IProjectTaskService
{
    Task<ProjectTaskDetailDto> GetProjectTaskById(long id, CancellationToken ct);
    IEnumerable<ProjectTaskSimpleDto> GetAll(long? projectId = null, ProjectTaskStatus? status = null, ProjectTaskPriority? priority = null);
    IEnumerable<ProjectTaskSimpleDto> GetByProjectId(long projectId);
    Task<ProjectTaskDetailDto> CreateAsync(CreateProjectTaskDto request, CancellationToken ct);
    Task<ProjectTaskDetailDto> UpdateAsync(UpdateProjectTaskDto request, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
}