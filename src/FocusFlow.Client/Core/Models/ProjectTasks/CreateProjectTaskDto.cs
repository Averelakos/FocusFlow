namespace FocusFlow.Client.Core.Models.ProjectTasks;

using FocusFlow.Client.Core.Models.Enums;

public class CreateProjectTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long ProjectId { get; set; }
    public long? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
    public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Low;
}
