public abstract class BaseEntity
{
    #region Common Properties
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? LastUpdated { get; set; }
    public long? LastUpdatedById { get; set; }
    #endregion Common Properties
    
    #region Navigation Properties
    public User? CreatedBy { get; set; }
    public User? LastUpdatedBy { get; set; }
    #endregion Navigation Properties
}