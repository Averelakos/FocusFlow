public interface IProjectTaskRepository
{
    // Define methods for task repository here
    IQueryable<ProjectTask> List();
    Task<ProjectTask?> GetAsync(long id, CancellationToken ct);
    Task<ProjectTask?> AddAsync(ProjectTask task, CancellationToken ct);
    Task<bool> UpdateAsync(ProjectTask task, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}