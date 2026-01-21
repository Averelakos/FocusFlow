public record ProjectTaskSimpleDto
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long ProjectId { get; init; }
    public DateTime? DueDate { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public ProjectTaskPriority Priority { get; init; }
}
