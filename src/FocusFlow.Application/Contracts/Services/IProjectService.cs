public interface IProjectService
{
    Task<ProjectDetailDto> GetProjectById(long id, CancellationToken ct);
    IEnumerable<ProjectSimpleDto> GetAll();
    Task<ProjectDetailDto> CreateAsync(CreateProjectDto request, CancellationToken ct);
    Task<ProjectDetailDto> UpdateAsync(UpdateProjectDto request, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
}

public interface IProjectTaskService
{
}