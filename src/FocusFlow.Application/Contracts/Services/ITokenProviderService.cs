using System.Security.Claims;

public interface ITokenProviderService
{
    string GenerateAccessToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}