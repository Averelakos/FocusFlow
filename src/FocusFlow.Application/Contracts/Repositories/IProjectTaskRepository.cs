using System.Linq.Expressions;

public interface IProjectTaskRepository
{
    IQueryable<ProjectTask> Queryable();
    Task<ProjectTask?> GetAsync(long id, CancellationToken ct);
    Task<ProjectTask?> AddAsync(ProjectTask task, CancellationToken ct);
    Task<bool> UpdateAsync(ProjectTask task, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<ProjectTask, bool>> predicate, CancellationToken ct);
}