namespace FocusFlow.Client.Core.Models.Projects;

public record ProjectSimpleDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
