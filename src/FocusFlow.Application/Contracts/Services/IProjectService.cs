public interface IProjectService
{
    Task<ProjectDetailDto> GetProjectById(long id, CancellationToken ct);
    IEnumerable<ProjectSimpleDto> GetAll();
    IEnumerable<ProjectLookupDto> GetLookup();
    Task<ProjectDetailDto> CreateAsync(CreateProjectDto request, CancellationToken ct);
    Task<ProjectDetailDto> UpdateAsync(UpdateProjectDto request, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
}
