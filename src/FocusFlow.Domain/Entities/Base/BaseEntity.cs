public abstract class BaseEntity
{
    #region Common Properties
    /// <summary>
    /// Unique identifier
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Timestamp when the entity was created
    /// </summary>
    public DateTime Created { get; set; }
    /// <summary>
    /// ID of the user who created this entity
    /// </summary>
    public long? CreatedById { get; set; }
    /// <summary>
    /// Timestamp when the entity was last updated
    /// </summary>
    public DateTime? LastUpdated { get; set; }
    /// <summary>
    /// ID of the user who last updated this entity
    /// </summary>
    public long? LastUpdatedById { get; set; }
    #endregion Common Properties
    
    #region Navigation Properties
    /// <summary>
    /// User who created this entity
    /// </summary>
    public User? CreatedBy { get; set; }
    /// <summary>
    /// User who last updated this entity
    /// </summary>
    public User? LastUpdatedBy { get; set; }
    #endregion Navigation Properties
}