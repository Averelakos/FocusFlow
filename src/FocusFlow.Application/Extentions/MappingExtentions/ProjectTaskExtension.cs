public static class ProjectTaskExtensions
{
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
