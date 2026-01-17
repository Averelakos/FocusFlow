public class User : BaseEntity
{
    #region Properties
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    #endregion Properties
    #region Navigation Properties
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    #endregion Navigation Properties
}