using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FocusFlowDbContext context, ILogger<UserRepository> logger)
        : base(logger, context)
    {
    }

    protected override IQueryable<User> SetWithIncludes()
    {
        return base.SetWithIncludes()
        .Include(x => x.OwnedProjects)
        .Include(x => x.AssignedTasks);
    }
}
