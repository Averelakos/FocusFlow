using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;

namespace FocusFlow.Client.Core.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        // Validate token expiration
        if (IsTokenExpired(token))
        {
            await _localStorage.RemoveItemAsync("authToken");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("authToken");
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        
        if (keyValuePairs == null)
            return Enumerable.Empty<Claim>();

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private bool IsTokenExpired(string token)
    {
        try
        {
            var claims = ParseClaimsFromJwt(token);
            var expClaim = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            
            if (string.IsNullOrEmpty(expClaim))
                return true;

            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
            return expDate < DateTimeOffset.UtcNow;
        }
        catch
        {
            return true;
        }
    }
}
