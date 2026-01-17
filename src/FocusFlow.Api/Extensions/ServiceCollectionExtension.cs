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
        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();

    }

    public static void AddProjects(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your projects here
    }
}