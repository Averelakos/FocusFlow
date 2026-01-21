using Microsoft.Extensions.Caching.Memory;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMemoryCache _cache;
    private const string ProjectLookupCacheKey = "ProjectLookupCache";

    public ProjectService(IProjectRepository projectRepository, ICurrentUserService currentUserService, IMemoryCache cache)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
        _cache = cache;
    }

    /// <summary>
    /// Gets a project by its ID with full details
    /// </summary>
    public async Task<ProjectDetailDto> GetProjectById(long id, CancellationToken ct)
    {
        var entity = await _projectRepository.GetAsync(id, ct);
        
        if (entity == null)
            throw new NotFoundException($"Project with id {id} not found");

        return entity.ToProjectDetailDto();
    }

    public IEnumerable<ProjectSimpleDto> GetAll()
    {
        return _projectRepository
        .Queryable()
        .Select(p => p.ToProjectSimpleDto())
        .ToList();
    }

    public IEnumerable<ProjectLookupDto> GetLookup()
    {
        // Try to get from cache
        if (_cache.TryGetValue(ProjectLookupCacheKey, out List<ProjectLookupDto>? cachedLookup))
        {
            return cachedLookup!;
        }

        // Fetch from database
        var lookup = _projectRepository
            .Queryable()
            .Select(p => p.ToProjectLookupDto())
            .ToList();

        // Store in cache
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(24));
        
        _cache.Set(ProjectLookupCacheKey, lookup, cacheOptions);

        return lookup;
    }

    /// <summary>
    /// Creates a new project and invalidates the lookup cache
    /// </summary>
    public async Task<ProjectDetailDto> CreateAsync(CreateProjectDto request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var project = request.ToEntity();
        project.OwnerId = userId.Value;
        
        var newEntity = await _projectRepository.AddAsync(project, ct);
        
        // Reload the entity with navigation properties
        var projectWithIncludes = await _projectRepository.GetAsync(newEntity.Id, ct);
        if (projectWithIncludes == null)
            throw new NotFoundException($"Project with id {newEntity.Id} not found after creation");
        
        // Invalidate cache since a new project was created
        _cache.Remove(ProjectLookupCacheKey);
            
        return projectWithIncludes.ToProjectDetailDto();
    }

    /// <summary>
    /// Updates an existing project and invalidates the lookup cache
    /// </summary>
    public async Task<ProjectDetailDto> UpdateAsync(UpdateProjectDto request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var entity = await _projectRepository.GetAsync(request.Id, ct);
        
        if (entity == null)
            throw new NotFoundException($"Project with id {request.Id} not found");
        
        // Verify the user owns the project
        if (entity.OwnerId != userId.Value)
            throw new ForbiddenException("You do not have permission to update this project");
        
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.LastUpdated = DateTime.UtcNow;
        entity.LastUpdatedById = userId.Value;
        
        await _projectRepository.UpdateAsync(entity, ct);
        
        // Invalidate cache since project name might have changed
        _cache.Remove(ProjectLookupCacheKey);
        
        return entity.ToProjectDetailDto();
    }

    /// <summary>
    /// Deletes a project and invalidates the lookup cache
    /// </summary>
    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var entity = await _projectRepository.GetAsync(id, ct);
        
        if (entity == null)
            throw new NotFoundException($"Project with id {id} not found");
        
        // Verify the user owns the project
        if (entity.OwnerId != userId.Value)
            throw new ForbiddenException("You do not have permission to delete this project");
        
        await _projectRepository.DeleteAsync(id, ct);
        
        // Invalidate cache since a project was deleted
        _cache.Remove(ProjectLookupCacheKey);
    }
}