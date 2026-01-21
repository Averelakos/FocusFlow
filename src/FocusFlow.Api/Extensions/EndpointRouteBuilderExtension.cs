using FocusFlow.Application.Hubs;

public static class EndpointRouteBuilderExtension
{
    public static void ConfigurationEndpointsRoute(this IEndpointRouteBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
           app.MapOpenApi();
        }
        
        // Map SignalR Hub
        app.MapHub<TaskHub>("/taskhub");
    }
}