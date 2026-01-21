namespace FocusFlow.Client.Core.Models.Projects;

public record ProjectLookupDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
