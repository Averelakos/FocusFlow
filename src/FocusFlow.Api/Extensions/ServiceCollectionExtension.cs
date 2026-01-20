using FocusFlow.Application;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your services here
        services.AddControllers();
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
                            .AllowAnyMethod();
        }));
        // services.AddCors(options =>
        // {
        //     options.AddPolicy("AllowBlazorClient", policy =>
        //     {
        //         policy.WithOrigins("http://localhost:5103", "https://localhost:7028")
        //               .AllowAnyMethod()
        //               .AllowAnyHeader();
        //     });
        // });
    }

    public static void AddProjects(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your projects here
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
    }
}