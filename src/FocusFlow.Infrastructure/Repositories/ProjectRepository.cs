using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(FocusFlowDbContext context, ILogger<ProjectRepository> logger)
        : base(logger, context)
    {
    }

    protected override IQueryable<Project> SetWithIncludes()
    {
        return base.SetWithIncludes()
        .Include(x => x.Owner)
        .Include(x => x.Tasks);
    }
}
