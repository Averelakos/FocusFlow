using System.Net.Http.Json;
using FocusFlow.Client.Core.Models.Projects;

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

    public async Task<ProjectDetailDto?> GetById(long id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailDto>($"api/project/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectDetailDto?> CreateAsync(CreateProjectDto createDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/project/create", createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectDetailDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating project: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectDetailDto?> UpdateAsync(UpdateProjectDto updateDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/project/update", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectDetailDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating project: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/project/delete/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting project: {ex.Message}");
            return false;
        }
    }
}
