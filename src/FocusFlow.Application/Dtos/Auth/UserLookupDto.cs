namespace FocusFlow.Application.Dtos.Auth;

/// <summary>
/// Lightweight DTO for user selection in dropdowns
/// </summary>
public class UserLookupDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
