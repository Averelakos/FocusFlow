namespace FocusFlow.Client.Core.Models.Users;

/// <summary>
/// Lightweight user model for dropdowns
/// </summary>
public class UserLookupDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}
