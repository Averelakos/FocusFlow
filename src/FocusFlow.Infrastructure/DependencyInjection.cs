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
        return services;
    }

    /// <summary>
    /// Adds the Db context and sets the option
    /// for the server to connecto to database in sql server
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns>services</returns>
    public static IServiceCollection AddDatabaseSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddDbContext<FocusFlowDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionSqlServer")));
    }
}
