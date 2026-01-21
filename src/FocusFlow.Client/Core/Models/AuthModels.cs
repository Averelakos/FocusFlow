namespace FocusFlow.Client.Core.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
}

public class LoginDto
{
    // [Required]
    public string UsernameOrEmail { get; set; } = string.Empty;
    // [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    // [Required]
    public string FullName { get; set; } = string.Empty;
    // [Required]
    public string Username { get; set; } = string.Empty;
    //  [Required]
    public string Email { get; set; } = string.Empty;
    // [Required]
    public string Password { get; set; } = string.Empty;
    // [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}


