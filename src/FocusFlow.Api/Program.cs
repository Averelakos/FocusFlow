var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();

// Run migrations before starting the application
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
    await migrationService.StartAsync();
}

app.ConfigureApplication(app.Environment);
app.UseRouting();
app.UseCors();

// Health check endpoint for Docker
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.MapControllers();
app.ConfigurationEndpointsRoute(app.Environment);
app.Run();
