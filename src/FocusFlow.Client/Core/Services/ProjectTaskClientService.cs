using System.Net.Http.Json;
using FocusFlow.Client.Core.Models.ProjectTasks;

namespace FocusFlow.Client.Services;

public class ProjectTaskClientService
{
    private readonly HttpClient _httpClient;

    public ProjectTaskClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectTaskSimpleDto>?> GetAll()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectTaskSimpleDto>>("api/projecttask/getall");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project tasks: {ex.Message}");
            return null;
        }
    }

    public async Task<List<ProjectTaskSimpleDto>?> GetByProjectId(long projectId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectTaskSimpleDto>>($"api/projecttask/project/{projectId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project tasks: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectTaskDetailDto?> GetById(long id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectTaskDetailDto>($"api/projecttask/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project task: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectTaskDetailDto?> CreateAsync(CreateProjectTaskDto createDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/projecttask/create", createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectTaskDetailDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating project task: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectTaskDetailDto?> UpdateAsync(UpdateProjectTaskDto updateDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/projecttask/update", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectTaskDetailDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating project task: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/projecttask/delete/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting project task: {ex.Message}");
            return false;
        }
    }
}
