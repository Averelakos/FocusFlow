public class Project: BaseEntity
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long OwnerId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    #endregion Properties
    
    #region Navigation Properties
    public User Owner { get; set; } = null!;
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    #endregion Navigation Properties
}