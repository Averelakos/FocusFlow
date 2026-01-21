public class ProjectTask : BaseEntity
{
    #region Properties
    /// <summary>
    /// Task title
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Task description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// ID of the project this task belongs to
    /// </summary>
    public long ProjectId { get; set; }     
    /// <summary>
    /// ID of the user assigned to this task
    /// </summary>
    public long AssignedToId { get; set; }
    /// <summary>
    /// Task due date
    /// </summary>
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// Date when the task was completed
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    /// <summary>
    /// Current status of the task (Todo, InProgress, Done)
    /// </summary>
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
    /// <summary>
    /// Priority level of the task (Low, Medium, High, Critical)
    /// </summary>
    public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Low;
    #endregion Properties
   
   #region Navigation Properties
    /// <summary>
    /// Project this task belongs to
    /// </summary>
    public Project Project { get; set; } = null!;
    /// <summary>
    /// User assigned to this task
    /// </summary>
    public User AssignedTo { get; set; } = null!;
   #endregion Navigation Properties
}