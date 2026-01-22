using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FocusFlow.Application.Validators;

namespace FocusFlow.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds the application services.
    /// </summary> <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddService();
        services.AddValidators();
        return services;
    }
    
    /// <summary>
    /// Adds the services.
    /// </summary> <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IProjectTaskService, ProjectTaskService>();
        services.AddTransient<IUserService, UserService>();
        return services;
    }

    /// <summary>
    /// Adds the validators.
    /// </summary> <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        return services;
    }
}
