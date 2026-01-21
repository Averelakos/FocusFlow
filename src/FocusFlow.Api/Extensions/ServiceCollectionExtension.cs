using FocusFlow.Application;
using FluentValidation;
using FluentValidation.AspNetCore;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds the services.
    /// </summary> <param name="services">The services.</param>
    /// <returns></returns>
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddMemoryCache();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        
        services.AddProjects(configuration);
        services.AddOpenApi(options =>
        {
           options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
        });
        
        // Add CORS
        services.AddCors(options => options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:5103", "https://localhost:7028")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
        }));
        
        services.AddSignalR();
    }
    
    /// <summary>
    /// Adds the projects.
    /// </summary> <param name="services">The services.</param>
    /// <returns></returns>
    public static void AddProjects(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
    }
}