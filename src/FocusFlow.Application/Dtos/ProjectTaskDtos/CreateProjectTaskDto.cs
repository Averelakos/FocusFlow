public record CreateProjectTaskDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long ProjectId { get; init; }
    public long? AssignedToId { get; init; }
    public DateTime? DueDate { get; init; }
    public ProjectTaskStatus Status { get; init; } = ProjectTaskStatus.Todo;
    public ProjectTaskPriority Priority { get; init; } = ProjectTaskPriority.Low;
}
