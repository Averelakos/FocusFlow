public interface IProjectRepository
{
    // Define methods for project repository here
    IQueryable<Project> List();
    Task<Project?> GetAsync(long id, CancellationToken ct);
    Task<Project?> AddAsync(Project project, CancellationToken ct);
    Task<bool> UpdateAsync(Project project, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}
