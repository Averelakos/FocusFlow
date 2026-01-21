using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ProjectTaskRepository : BaseRepository<ProjectTask>, IProjectTaskRepository
{
    public ProjectTaskRepository(FocusFlowDbContext context, ILogger<ProjectTaskRepository> logger, ICurrentUserService currentUserService)
        : base(logger, context, currentUserService)
    {
    }

    /// <summary>
    /// Includes Project and AssignedTo navigation properties
    /// </summary>
    protected override IQueryable<ProjectTask> SetWithIncludes()
    {
        return base.SetWithIncludes()
        .Include(x => x.Project)
        .Include(x => x.AssignedTo);
    }
}