public class User : BaseEntity
{
    #region Properties
    /// <summary>
    /// Unique username for login
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// User's full name for display purposes
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Hashed password for authentication
    /// </summary>
    public required byte[] PasswordHash{ get; set; }
    /// <summary>
    /// Salt used for password hashing
    /// </summary>
    public required byte[] PasswordSalt { get; set; }
    #endregion Properties
    #region Navigation Properties
    /// <summary>
    /// Projects owned by this user
    /// </summary>
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    /// <summary>
    /// Tasks assigned to this user
    /// </summary>
    public ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
    #endregion Navigation Properties
}