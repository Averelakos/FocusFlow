namespace FocusFlow.Client.Core.Models.ProjectTasks;

public class ProjectTaskDetailDto : ProjectTaskSimpleDto
{
    public long AssignedToId { get; set; }
    public string AssignedToName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
