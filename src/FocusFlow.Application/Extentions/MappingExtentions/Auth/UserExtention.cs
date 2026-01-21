using System.Security.Cryptography;
using System.Text;


public static class UserExtensions
{
    /// <summary>
    /// Converts a RegisterDto to a User entity with hashed password
    /// </summary>
    /// <param name="dto">The registration data transfer object</param>
    /// <returns>User entity with hashed password and salt</returns>
    public static User ToEntity(this RegisterDto dto)
    {
        using var hmac = new HMACSHA512();
        return new User
        {
            FullName = dto.FullName,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key,
            CreatedById = -1,
            LastUpdatedById = -1
        };
    }

    /// <summary>
    /// Checks if the provided string matches the user's username or email (case-insensitive)
    /// </summary>
    /// <param name="user">The user to check against</param>
    /// <param name="usernameOrEmail">The username or email to verify</param>
    /// <returns>True if the string matches either username or email</returns>
    public static bool IsUsernameOrEmail(this User user, string usernameOrEmail)
    {
        return user.Username.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase) ||
               user.Email.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifies a password against the user's stored password hash
    /// </summary>
    /// <param name="user">The user whose password to verify</param>
    /// <param name="password">The plain text password to verify</param>
    /// <returns>True if the password matches the stored hash</returns>
    public static bool VerifyPassword(this User user, string password)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(user.PasswordHash);
    }

}