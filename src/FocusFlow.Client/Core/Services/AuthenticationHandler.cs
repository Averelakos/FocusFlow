using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace FocusFlow.Client.Core.Services;

public class AuthenticationHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public AuthenticationHandler(ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _localStorage.RemoveItemAsync("authToken");
            _navigationManager.NavigateTo("/login");
        }

        return response;
    }
}
