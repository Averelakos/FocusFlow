using FocusFlow.Application.Hubs;

public static class EndpointRouteBuilderExtension
{
    public static void ConfigurationEndpointsRoute(this IEndpointRouteBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
           app.MapOpenApi();
           
        // app.MapGet("/openapi/v1", () => Results.Redirect("/swagger/v1/swagger.json"));
        }
        
        // Map SignalR Hub
        app.MapHub<TaskHub>("/taskhub");
    }
}