var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();
app.ConfigureApplication(app.Environment);
app.UseRouting();
app.UseCors();
app.MapControllers();
app.ConfigurationEndpointsRoute(app.Environment);
app.Run();
