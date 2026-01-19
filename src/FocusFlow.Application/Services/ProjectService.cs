public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
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
        var newEntity = await _projectRepository.AddAsync(request.ToEntity(), ct);
        return newEntity.ToProjectDetailDto();
    }

    public async Task<ProjectDetailDto> UpdateAsync(UpdateProjectDto request, CancellationToken ct)
    {
        var entity = await _projectRepository.GetAsync(request.Id, ct);
        
        if (entity == null)
            throw new NotFoundException($"Project with id {request.Id} not found");
        
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.LastUpdated = DateTime.UtcNow;
        // entity.LastUpdatedById = null; // ToDo: Set the user id from the context
        
        await _projectRepository.UpdateAsync(entity, ct);
        return entity.ToProjectDetailDto();
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var entity = await _projectRepository.GetAsync(id, ct);
        
        if (entity == null)
            throw new NotFoundException($"Project with id {id} not found");
        
        await _projectRepository.DeleteAsync(id, ct);
    }
}