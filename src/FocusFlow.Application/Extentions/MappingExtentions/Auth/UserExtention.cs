using System.Security.Cryptography;
using System.Text;

public static class UserExtensions
{
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

    public static bool IsUsernameOrEmail(this User user, string usernameOrEmail)
    {
        return user.Username.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase) ||
               user.Email.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase);
    }

    public static bool VerifyPassword(this User user, string password)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(user.PasswordHash);
    }

//     public static bool IsEmail(string input)
// {
//     try
//     {
//         var mailAddress = new MailAddress(input);
//         return true;
//     }
//     catch
//     {
//         return false;
//     }
// }
}