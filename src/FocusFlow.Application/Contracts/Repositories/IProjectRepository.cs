using System.Linq.Expressions;

public interface IProjectRepository
{
    IQueryable<Project> Queryable();
    Task<Project?> GetAsync(long id, CancellationToken ct);
    Task<Project?> AddAsync(Project project, CancellationToken ct);
    Task<bool> UpdateAsync(Project project, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<Project, bool>> predicate, CancellationToken ct);
}
