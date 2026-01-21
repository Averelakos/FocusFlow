using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FocusFlow.Application.Validators;

namespace FocusFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add application services here
        services.AddService();
        services.AddValidators();
        return services;
    }

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IProjectTaskService, ProjectTaskService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        return services;
    }
}
