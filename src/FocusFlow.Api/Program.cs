var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();
app.ConfigurationEndpointsRoute(app.Environment);
app.ConfigureApplication(app.Environment);
app.MapControllers();
app.Run();
