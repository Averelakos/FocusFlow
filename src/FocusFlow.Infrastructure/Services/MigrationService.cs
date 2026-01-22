using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

public class MigrationService : IMigrationService
{
    private readonly FocusFlowDbContext _focusFlowDbContext;
    private readonly MigrationConfigurations _migrationsConfigurations;

    public MigrationService(FocusFlowDbContext focusFlowDbContext, IOptions<MigrationConfigurations> migrationsConfigurations)
    {
        _focusFlowDbContext = focusFlowDbContext;
        _migrationsConfigurations = migrationsConfigurations.Value;
    }

    public async Task StartAsync()
    {
        if (_migrationsConfigurations.Enabled is false)
            return;

        RelationalDatabaseCreator creator = (_focusFlowDbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator)!;
        var hasCreated = await creator.ExistsAsync();
        if (!hasCreated)
            await creator.CreateAsync();

        var migrationsPending = _focusFlowDbContext.Database.GetPendingMigrations();

        if (migrationsPending.Any())
            await _focusFlowDbContext.Database.MigrateAsync();
    }
}