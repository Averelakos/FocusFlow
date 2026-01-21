public record UpdateProjectTaskDto
{
    public long Id { get; set; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long? AssignedToId { get; init; }
    public DateTime? DueDate { get; init; }
    public ProjectTaskStatus? Status { get; init; }
    public ProjectTaskPriority? Priority { get; init; }
}
