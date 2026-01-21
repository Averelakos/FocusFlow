using System.Net.Http.Json;
using FocusFlow.Client.Core.Models;
using Blazored.LocalStorage;

namespace FocusFlow.Client.Core.Services;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginDto request);
    Task<AuthResponse> Register(RegisterDto request);
    Task Logout();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<AuthResponse> Login(LoginDto request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Invalid response from server" };
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = $"Login failed: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> Register(RegisterDto request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Invalid response from server" };
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = $"Registration failed: {ex.Message}" };
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
    }
}
