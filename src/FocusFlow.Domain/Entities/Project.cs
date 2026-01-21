public class Project: BaseEntity
{
    #region Properties
    /// <summary>
    /// Project name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Project description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// ID of the user who owns this project
    /// </summary>
    public long OwnerId { get; set; }
    /// <summary>
    /// Project start date
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// Project end date
    /// </summary>
    public DateTime? EndDate { get; set; }
    #endregion Properties
    
    #region Navigation Properties
    /// <summary>
    /// User who owns this project
    /// </summary>
    public User Owner { get; set; } = null!;
    /// <summary>
    /// Collection of tasks belonging to this project
    /// </summary>
    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    #endregion Navigation Properties
}