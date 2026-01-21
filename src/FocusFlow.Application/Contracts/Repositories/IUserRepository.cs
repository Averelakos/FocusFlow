using System.Linq.Expressions;

public interface IUserRepository
{
    IQueryable<User> Queryable();
    Task<User?> GetAsync(long id, CancellationToken ct);
    Task<User?> GetByParameterAsync(Expression<Func<User, bool>> predicate, CancellationToken ct);
    Task<User?> AddAsync(User user, CancellationToken ct);
    Task<bool> UpdateAsync(User user, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken ct);
}
