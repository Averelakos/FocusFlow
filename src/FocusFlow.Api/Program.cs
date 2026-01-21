var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();

app.ConfigureApplication(app.Environment);
// app.UseHttpsRedirection();

// app.UseBlazorFrameworkFiles();
// app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors();

// app.MapRazorPages();
app.MapControllers();

// Map SignalR hub after CORS
app.ConfigurationEndpointsRoute(app.Environment);

// app.MapFallbackToFile("index.html");
app.Run();
