namespace FocusFlow.Client.Core.Models.ProjectTasks;

public record ProjectTaskStatisticsDto
{
    public int TotalTasks { get; init; }
    public int CompletedTasks { get; init; }
    public int OverdueTasks { get; init; }
    public int InProgressTasks { get; init; }
    public int TodoTasks { get; init; }
}

public record ProjectStatisticsDto
{
    public long ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public int TotalTasks { get; init; }
    public int CompletedTasks { get; init; }
    public int OverdueTasks { get; init; }
    public int InProgressTasks { get; init; }
    public int TodoTasks { get; init; }
}
