var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();
app.ConfigurationEndpointsRoute(app.Environment);
app.ConfigureApplication(app.Environment);
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("AllowBlazorClient");

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
