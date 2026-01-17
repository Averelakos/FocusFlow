using Microsoft.EntityFrameworkCore;

public sealed partial class FocusFlowDbContext: DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ModelCreating(modelBuilder);
    }

    public void ModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}