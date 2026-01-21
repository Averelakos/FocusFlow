using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add infrastructure services here
        services.AddDatabaseSqlServer(configuration);
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    /// <summary>
    /// Adds the Db context and sets the option
    /// for the server to connect to SQL Server database
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns>services</returns>
    public static IServiceCollection AddDatabaseSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddDbContext<FocusFlowDbContext>(options => 
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
        services.AddTransient<IProjectRepository, ProjectRepository>();
        services.AddTransient<IProjectTaskRepository, ProjectTaskRepository>();
        return services;
    }
    
    /// <summary>
    /// Adds the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ITokenProviderService, TokenProviderService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
