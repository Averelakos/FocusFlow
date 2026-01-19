public interface IUserRepository
{
    // Define methods for user repository here
    IQueryable<User> List();
    Task<User?> GetAsync(long id, CancellationToken ct);
    Task<User?> AddAsync(User user, CancellationToken ct);
    Task<bool> UpdateAsync(User user, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}
