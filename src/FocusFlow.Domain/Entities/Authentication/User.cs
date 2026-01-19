public class User : BaseEntity
{
    #region Properties
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public required byte[] PasswordHash{ get; set; }
    public required byte[] PasswordSalt { get; set; }
    #endregion Properties
    #region Navigation Properties
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    public ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
    #endregion Navigation Properties
}