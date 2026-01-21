using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(FocusFlowDbContext context, ILogger<ProjectRepository> logger, ICurrentUserService currentUserService)
        : base(logger, context, currentUserService)
    {
    }

    /// <summary>
    /// Includes Owner, CreatedBy, LastUpdatedBy, and Tasks navigation properties
    /// </summary>
    protected override IQueryable<Project> SetWithIncludes()
    {
        return base.SetWithIncludes()
        .Include(x => x.Owner)
        .Include(x => x.CreatedBy)
        .Include(x => x.LastUpdatedBy)
        .Include(x => x.Tasks);
    }
}
