namespace FocusFlow.Client.Core.Models.ProjectTasks;


public record ProjectTaskStatisticsDto
{
    /// <summary>Total number of tasks</summary>
    public int TotalTasks { get; init; }
    /// <summary>Number of tasks marked as Done</summary>
    public int CompletedTasks { get; init; }
    /// <summary>Number of tasks past their due date and not completed</summary>
    public int OverdueTasks { get; init; }
    /// <summary>Number of tasks currently in progress</summary>
    public int InProgressTasks { get; init; }
    /// <summary>Number of tasks not yet started</summary>
    public int TodoTasks { get; init; }
}


public record ProjectStatisticsDto
{
    /// <summary>The project's unique identifier</summary>
    public long ProjectId { get; init; }
    /// <summary>The project's name</summary>
    public string ProjectName { get; init; } = string.Empty;
    /// <summary>Total number of tasks in the project</summary>
    public int TotalTasks { get; init; }
    /// <summary>Number of completed tasks in the project</summary>
    public int CompletedTasks { get; init; }
    /// <summary>Number of overdue tasks in the project</summary>
    public int OverdueTasks { get; init; }
    /// <summary>Number of in-progress tasks in the project</summary>
    public int InProgressTasks { get; init; }
    /// <summary>Number of to-do tasks in the project</summary>
    public int TodoTasks { get; init; }
}
