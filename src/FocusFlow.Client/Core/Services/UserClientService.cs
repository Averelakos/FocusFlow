using FocusFlow.Client.Core.Models.Users;
using System.Net.Http.Json;

namespace FocusFlow.Client.Core.Services;

/// <summary>
/// Service for interacting with the User API
/// </summary>
public class UserClientService
{
    private readonly HttpClient _httpClient;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets a lightweight list of users for dropdowns (ID and FullName only)
    /// </summary>
    public async Task<List<UserLookupDto>?> GetLookup()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<UserLookupDto>>("api/user/lookup");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            return null;
        }
    }
}
