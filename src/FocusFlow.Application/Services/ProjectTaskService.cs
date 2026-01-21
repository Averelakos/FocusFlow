public class ProjectTaskService : IProjectTaskService
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ICurrentUserService _currentUserService;

    public ProjectTaskService(IProjectTaskRepository projectTaskRepository, ICurrentUserService currentUserService)
    {
        _projectTaskRepository = projectTaskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ProjectTaskDetailDto> GetProjectTaskById(long id, CancellationToken ct)
    {
        var entity = await _projectTaskRepository.GetAsync(id, ct);
        
        if (entity == null)
            throw new NotFoundException($"ProjectTask with id {id} not found");

        return entity.ToProjectTaskDetailDto();
    }

    public IEnumerable<ProjectTaskSimpleDto> GetAll(long? projectId = null, ProjectTaskStatus? status = null, ProjectTaskPriority? priority = null)
    {
        var query = _projectTaskRepository.Queryable();

        if (projectId.HasValue)
            query = query.Where(pt => pt.ProjectId == projectId.Value);

        if (status.HasValue)
            query = query.Where(pt => pt.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(pt => pt.Priority == priority.Value);

        return query
            .Select(pt => pt.ToProjectTaskSimpleDto())
            .ToList();
    }

    public IEnumerable<ProjectTaskSimpleDto> GetByProjectId(long projectId)
    {
        return _projectTaskRepository
        .Queryable()
        .Where(pt => pt.ProjectId == projectId)
        .Select(pt => pt.ToProjectTaskSimpleDto())
        .ToList();
    }

    public async Task<ProjectTaskDetailDto> CreateAsync(CreateProjectTaskDto request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var projectTask = request.ToEntity();
        
        // If no assignee specified, assign to current user
        if (projectTask.AssignedToId == 0)
            projectTask.AssignedToId = userId.Value;
        
        var newEntity = await _projectTaskRepository.AddAsync(projectTask, ct);
        
        if (newEntity == null)
            throw new NotFoundException("Failed to create project task");
        
        // Reload the entity with navigation properties
        var projectTaskWithIncludes = await _projectTaskRepository.GetAsync(newEntity.Id, ct);
        if (projectTaskWithIncludes == null)
            throw new NotFoundException($"ProjectTask with id {newEntity.Id} not found after creation");
            
        return projectTaskWithIncludes.ToProjectTaskDetailDto();
    }

    public async Task<ProjectTaskDetailDto> UpdateAsync(UpdateProjectTaskDto request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var entity = await _projectTaskRepository.GetAsync(request.Id, ct);
        
        if (entity == null)
            throw new NotFoundException($"ProjectTask with id {request.Id} not found");
        
        entity.Title = request.Title;
        entity.Description = request.Description ?? string.Empty;
        entity.DueDate = request.DueDate;
        
        if (request.AssignedToId.HasValue)
            entity.AssignedToId = request.AssignedToId.Value;
        
        if (request.Status.HasValue)
            entity.Status = request.Status.Value;
        
        if (request.Priority.HasValue)
            entity.Priority = request.Priority.Value;
        
        entity.LastUpdated = DateTime.UtcNow;
        entity.LastUpdatedById = userId.Value;
        
        await _projectTaskRepository.UpdateAsync(entity, ct);
        return entity.ToProjectTaskDetailDto();
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            throw new UnauthorizedException("User not authenticated");

        var entity = await _projectTaskRepository.GetAsync(id, ct);
        
        if (entity == null)
            throw new NotFoundException($"ProjectTask with id {id} not found");
        
        await _projectTaskRepository.DeleteAsync(id, ct);
    }
}
