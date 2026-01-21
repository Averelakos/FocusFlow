namespace FocusFlow.Client.Core.Models.ProjectTasks;

using FocusFlow.Client.Core.Models.Enums;

public class UpdateProjectTaskDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public ProjectTaskStatus? Status { get; set; }
    public ProjectTaskPriority? Priority { get; set; }
}
