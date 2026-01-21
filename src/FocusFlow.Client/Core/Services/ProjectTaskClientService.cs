using System.Net.Http.Json;
using FocusFlow.Client.Core.Models.ProjectTasks;
using FocusFlow.Client.Core.Models.Enums;

namespace FocusFlow.Client.Services;

public class ProjectTaskClientService
{
    private readonly HttpClient _httpClient;

    public ProjectTaskClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all project tasks with optional filtering by project, status, and priority
    /// </summary>
    public async Task<List<ProjectTaskSimpleDto>?> GetAll(long? projectId = null, ProjectTaskStatus? status = null, ProjectTaskPriority? priority = null)
    {
        try
        {
            var queryParams = new List<string>();
            
            if (projectId.HasValue)
                queryParams.Add($"projectId={projectId.Value}");
            
            if (status.HasValue)
                queryParams.Add($"status={status.Value}");
            
            if (priority.HasValue)
                queryParams.Add($"priority={priority.Value}");
            
            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            
            return await _httpClient.GetFromJsonAsync<List<ProjectTaskSimpleDto>>($"api/projecttask/getall{queryString}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project tasks: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets all tasks for a specific project
    /// </summary>
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

    /// <summary>
    /// Gets a project task by its ID with full details
    /// </summary>
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

    /// <summary>
    /// Creates a new project task
    /// </summary>
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

    /// <summary>
    /// Updates an existing project task
    /// </summary>
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

    /// <summary>
    /// Deletes a project task by its ID
    /// </summary>
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

    /// <summary>
    /// Gets overall task statistics (total, completed, overdue, in progress, to-do)
    /// </summary>
    public async Task<ProjectTaskStatisticsDto?> GetStatistics()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectTaskStatisticsDto>("api/projecttask/statistics");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets task statistics grouped by project
    /// </summary>
    public async Task<List<ProjectStatisticsDto>?> GetProjectStatistics()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectStatisticsDto>>("api/projecttask/statistics/byproject");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project statistics: {ex.Message}");
            return null;
        }
    }
}
