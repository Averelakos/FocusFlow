
public static class ProjectExtensions
{
    /// <summary>
    /// Converts a Project entity to a detailed DTO with full information
    /// </summary>
    /// <param name="entity">The project entity to convert</param>
    /// <returns>ProjectDetailDto with complete project information including owner details</returns>
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

    /// <summary>
    /// Converts a Project entity to a simple DTO with basic information
    /// </summary>
    /// <param name="entity">The project entity to convert</param>
    /// <returns>ProjectSimpleDto with ID, name, and description</returns>
    public static ProjectSimpleDto ToProjectSimpleDto(this Project entity)
    {
        return new ProjectSimpleDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    /// <summary>
    /// Converts a Project entity to a lightweight lookup DTO for dropdowns
    /// </summary>
    /// <param name="entity">The project entity to convert</param>
    /// <returns>ProjectLookupDto with only ID and name</returns>
    public static ProjectLookupDto ToProjectLookupDto(this Project entity)
    {
        return new ProjectLookupDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    /// <summary>
    /// Converts a CreateProjectDto to a Project entity
    /// </summary>
    /// <param name="dto">The create project data transfer object</param>
    /// <returns>Project entity with data from the DTO</returns>
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