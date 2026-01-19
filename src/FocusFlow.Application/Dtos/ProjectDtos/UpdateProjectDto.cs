public record UpdateProjectDto
{
    public long Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}