public record ProjectTaskDetailDto : ProjectTaskSimpleDto
{
    public long AssignedToId { get; init; }
    public string AssignedToName { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}
