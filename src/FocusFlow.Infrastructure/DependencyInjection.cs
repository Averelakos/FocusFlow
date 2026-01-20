using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add infrastructure services here
        services.AddDatabaseSqlite(configuration);
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    /// <summary>
    /// Adds the Db context and sets the option
    /// for the server to connect to SQLite database
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns>services</returns>
    public static IServiceCollection AddDatabaseSqlite(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddDbContext<FocusFlowDbContext>(options => 
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            options.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });
    }

    /// <summary>
    /// Adds the repositories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register repositories here
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IProjectTaskRepository, ProjectTaskRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenProviderService, TokenProviderService>();
        return services;
    }
}
