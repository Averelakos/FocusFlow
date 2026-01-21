namespace FocusFlow.Client.Core.Models.Projects;

public record UpdateProjectDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
