public class ProjectTask : BaseEntity
{
    #region Properties
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long ProjectId { get; set; }     
    public long AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    #endregion Properties
   
   #region Navigation Properties
    public Project Project { get; set; } = null!;
    public User AssignedTo { get; set; } = null!;
   #endregion Navigation Properties
}