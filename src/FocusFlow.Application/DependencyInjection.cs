using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add application services here
        services.AddService();
        return services;
    }

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IProjectService, ProjectService>();
        return services;
    }
}
