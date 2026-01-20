using System.Net.Http.Json;

namespace FocusFlow.Client.Services;

public class ProjectClientService
{
    private readonly HttpClient _httpClient;

    public ProjectClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectSimpleDto>?> GetAll()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectSimpleDto>>("api/project/getall");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching projects: {ex.Message}");
            return null;
        }
    }

    // Example: Get projects
    // public async Task<List<ProjectDto>?> GetProjectsAsync()
    // {
    //     try
    //     {
    //         return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error fetching projects: {ex.Message}");
    //         return null;
    //     }
    // }

    // // Example: Create project
    // public async Task<ProjectDto?> CreateProjectAsync(CreateProjectDto createDto)
    // {
    //     try
    //     {
    //         var response = await _httpClient.PostAsJsonAsync("api/projects", createDto);
    //         response.EnsureSuccessStatusCode();
    //         return await response.Content.ReadFromJsonAsync<ProjectDto>();
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error creating project: {ex.Message}");
    //         return null;
    //     }
    // }

public async Task<string?> TestApiAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>("api/project/test");
            if (response != null && response.ContainsKey("token"))
            {
                return response["token"];
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling test API: {ex.Message}");
            return null;
        }
    }
    // Add more methods for your other API endpoints
}
