public interface IProjectTaskService
{
    Task<ProjectTaskDetailDto> GetProjectTaskById(long id, CancellationToken ct);
    IEnumerable<ProjectTaskSimpleDto> GetAll(long? projectId = null, ProjectTaskStatus? status = null, ProjectTaskPriority? priority = null);
    IEnumerable<ProjectTaskSimpleDto> GetByProjectId(long projectId);
    ProjectTaskStatisticsDto GetStatistics();
    IEnumerable<ProjectStatisticsDto> GetProjectStatistics();
    Task<ProjectTaskDetailDto> CreateAsync(CreateProjectTaskDto request, CancellationToken ct);
    Task<ProjectTaskDetailDto> UpdateAsync(UpdateProjectTaskDto request, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
}