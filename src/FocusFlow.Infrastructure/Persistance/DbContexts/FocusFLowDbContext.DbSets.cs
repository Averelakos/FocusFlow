using Microsoft.EntityFrameworkCore;

public sealed partial class FocusFlowDbContext: DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Task> Tasks => Set<Task>();
}