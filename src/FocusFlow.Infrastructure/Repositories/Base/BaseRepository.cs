using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly ILogger<BaseRepository<T>> _logger;
    protected readonly FocusFlowDbContext _focusFlowDbContext;
    public BaseRepository(ILogger<BaseRepository<T>> logger, FocusFlowDbContext focusFlowDbContext)
    {
        _logger = logger;
        _focusFlowDbContext = focusFlowDbContext;
    }

    protected virtual DbSet<T> Set()
    {
        return _focusFlowDbContext.Set<T>();
    }

    protected virtual IQueryable<T> SetWithIncludes()
    {
        return Set();
    }

    public virtual IQueryable<T> Queryable()
    {
        return Set().AsNoTracking();
    }

    public virtual async Task<T?> GetAsync(long id, CancellationToken ct)
    {
        return await Set().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public virtual async Task<T?> GetByParameterAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await Set().AsNoTracking().FirstOrDefaultAsync(predicate, ct);
    }

    public virtual async Task<T?> AddAsync(T entity, CancellationToken ct)
    {
        var result = await Set().AddAsync(entity, ct);
        if (await SaveChangesAsync(ct) < 1)
        {
            return null;
        }
        return await GetAsync(result.Entity.Id, ct);
    }

    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken ct)
    {
        // ToDO: Consider using a more sophisticated approach to handle concurrency and partial updates.
        Set().Update(entity);
        return await SaveChangesAsync(ct) > 0;
    }

    public virtual async Task<bool> DeleteAsync(long id, CancellationToken ct)
    {
        var entity = await GetAsync(id, ct) 
        ?? throw new InvalidOperationException($"Entity with id {id} not found.");

        await CustomDeleteAsync(entity, ct);
        
        try
        {
            Set().Remove(entity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting entity with id {Id}.", id);
            throw;
        }
        
        return await SaveChangesAsync(ct) > 0;
    }

    public virtual async Task CustomDeleteAsync(T entity, CancellationToken ct)
    {
        await Task.FromResult(true);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        try
        {
            return await _focusFlowDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database.");
            throw;
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await _focusFlowDbContext.Set<T>().AnyAsync(predicate, ct);
    }
}