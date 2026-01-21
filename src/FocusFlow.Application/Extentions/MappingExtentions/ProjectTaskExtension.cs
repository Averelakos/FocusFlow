
public static class ProjectTaskExtensions
{
    /// <summary>
    /// Converts a ProjectTask entity to a detailed DTO with full information
    /// </summary>
    /// <param name="entity">The project task entity to convert</param>
    /// <returns>ProjectTaskDetailDto with complete task information including project and assignee details</returns>
    public static ProjectTaskDetailDto ToProjectTaskDetailDto(this ProjectTask entity)
    {
        return new ProjectTaskDetailDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.ProjectId,
            ProjectName = entity.Project.Name,
            AssignedToId = entity.AssignedToId,
            AssignedToName = entity.AssignedTo.FullName,
            DueDate = entity.DueDate,
            Status = entity.Status,
            Priority = entity.Priority,
            CompletedAt = entity.CompletedAt,
            CreatedAt = entity.Created,
            UpdatedAt = entity.LastUpdated
        };
    }

    /// <summary>
    /// Converts a ProjectTask entity to a simple DTO with basic information
    /// </summary>
    /// <param name="entity">The project task entity to convert</param>
    /// <returns>ProjectTaskSimpleDto with ID, title, description, project ID, due date, status, and priority</returns>
    public static ProjectTaskSimpleDto ToProjectTaskSimpleDto(this ProjectTask entity)
    {
        return new ProjectTaskSimpleDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.ProjectId,
            DueDate = entity.DueDate,
            Status = entity.Status,
            Priority = entity.Priority
        };
    }

    /// <summary>
    /// Converts a CreateProjectTaskDto to a ProjectTask entity
    /// </summary>
    /// <param name="dto">The create project task data transfer object</param>
    /// <returns>ProjectTask entity with data from the DTO</returns>
    public static ProjectTask ToEntity(this CreateProjectTaskDto dto)
    {
        return new ProjectTask
        {
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            ProjectId = dto.ProjectId,
            AssignedToId = dto.AssignedToId ?? 0,
            DueDate = dto.DueDate,
            Status = dto.Status,
            Priority = dto.Priority
        };
    }
}
