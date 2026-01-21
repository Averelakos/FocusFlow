namespace FocusFlow.Client.Core.Models.ProjectTasks;

using FocusFlow.Client.Core.Models.Enums;

public class ProjectTaskSimpleDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long ProjectId { get; set; }
    public DateTime? DueDate { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public ProjectTaskPriority Priority { get; set; }
}
