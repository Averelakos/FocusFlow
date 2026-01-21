using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly ILogger<BaseRepository<T>> _logger;

    protected readonly ICurrentUserService _currentUserService;
    protected readonly FocusFlowDbContext _focusFlowDbContext;
    public BaseRepository(ILogger<BaseRepository<T>> logger, FocusFlowDbContext focusFlowDbContext, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _focusFlowDbContext = focusFlowDbContext;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Gets the DbSet for the entity type
    /// </summary>
    protected virtual DbSet<T> Set()
    {
        return _focusFlowDbContext.Set<T>();
    }

    /// <summary>
    /// Gets the queryable set with navigation properties included (override to customize includes)
    /// </summary>
    protected virtual IQueryable<T> SetWithIncludes()
    {
        return Set();
    }

    /// <summary>
    /// Gets a queryable set without tracking for read-only operations
    /// </summary>
    public virtual IQueryable<T> Queryable()
    {
        return Set().AsNoTracking();
    }

    /// <summary>
    /// Gets an entity by its ID with navigation properties included
    /// </summary>
    public virtual async Task<T?> GetAsync(long id, CancellationToken ct)
    {
        return await SetWithIncludes().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    /// <summary>
    /// Gets an entity matching a specific condition
    /// </summary>
    public virtual async Task<T?> GetByParameterAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await Set().AsNoTracking().FirstOrDefaultAsync(predicate, ct);
    }

    /// <summary>
    /// Adds a new entity to the database with audit fields
    /// </summary>
    public virtual async Task<T?> AddAsync(T entity, CancellationToken ct)
    {
        entity.Created = DateTime.UtcNow;
        entity.CreatedById = _currentUserService.GetUserId();

        var result = await Set().AddAsync(entity, ct);
        if (await SaveChangesAsync(ct) < 1)
        {
            return null;
        }
        return await GetAsync(result.Entity.Id, ct);
    }

    /// <summary>
    /// Updates an existing entity with audit fields
    /// </summary>
    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken ct)
    {
        // ToDO: Consider using a more sophisticated approach to handle concurrency and partial updates.
        entity.LastUpdated = DateTime.UtcNow;
        entity.LastUpdatedById = _currentUserService.GetUserId();
        
        Set().Update(entity);
        return await SaveChangesAsync(ct) > 0;
    }

    /// <summary>
    /// Deletes an entity by its ID
    /// </summary>
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

    /// <summary>
    /// Hook for custom delete logic before entity removal (override for cascading deletes)
    /// </summary>
    public virtual async Task CustomDeleteAsync(T entity, CancellationToken ct)
    {
        await Task.FromResult(true);
    }

    /// <summary>
    /// Saves changes to the database
    /// </summary>
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

    /// <summary>
    /// Checks if any entity matches the specified condition
    /// </summary>
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await _focusFlowDbContext.Set<T>().AnyAsync(predicate, ct);
    }
}