public static class ProjectExtensions
{
    public static ProjectDetailDto ToProjectDetailDto(this Project entity)
    {
        return new ProjectDetailDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            OwnerId = entity.OwnerId,
            OwnerName = entity.Owner.FullName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            CreatedAt = entity.Created,
            UpdatedAt = entity.LastUpdated
        };
    }

    public static ProjectSimpleDto ToProjectSimpleDto(this Project entity)
    {
        return new ProjectSimpleDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static Project ToEntity(this CreateProjectDto dto)
    {
        return new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }
}