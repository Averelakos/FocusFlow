public record CreateProjectDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int UserId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
