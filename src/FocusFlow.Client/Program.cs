using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using FocusFlow.Client;
using FocusFlow.Client.Services;
using FocusFlow.Client.Core.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Configure HttpClient with authentication handler
builder.Services.AddScoped<AuthenticationHandler>();

builder.Services.AddScoped(sp =>
{
    var authHandler = sp.GetRequiredService<AuthenticationHandler>();
    authHandler.InnerHandler = new HttpClientHandler();
    
    var httpClient = new HttpClient(authHandler)
    {
        BaseAddress = new Uri("http://localhost:5094")
    };
    
    return httpClient;
});

// Register authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register notification service
builder.Services.AddScoped<NotificationService>();

// Register existing API services
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<ProjectClientService>();

await builder.Build().RunAsync();
