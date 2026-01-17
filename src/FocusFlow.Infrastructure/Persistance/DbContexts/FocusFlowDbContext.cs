using Microsoft.EntityFrameworkCore;

public sealed partial class FocusFlowDbContext(DbContextOptions<FocusFlowDbContext> options): DbContext(options)    
{
    protected override void OnConfiguring(DbContextOptionsBuilder optiosconfigurationBuilder)
    {
        optiosconfigurationBuilder.EnableSensitiveDataLogging();
    }
}