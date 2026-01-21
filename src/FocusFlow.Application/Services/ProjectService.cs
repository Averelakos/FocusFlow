public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public ProjectService(IProjectRepository projectRepository, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

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

    public async Task<ProjectDetailDto> CreateAsync(CreateProjectDto request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var project = request.ToEntity();
        project.OwnerId = userId.Value;
        
        var newEntity = await _projectRepository.AddAsync(project, ct);
        return newEntity.ToProjectDetailDto();
    }

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
        return entity.ToProjectDetailDto();
    }

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
    }
}