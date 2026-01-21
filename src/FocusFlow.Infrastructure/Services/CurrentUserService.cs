using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        Console.WriteLine($"[CurrentUserService] User Identity: {user?.Identity?.Name}, IsAuthenticated: {user?.Identity?.IsAuthenticated}");
        
        // Debug: List all claims
        if (user != null)
        {
            Console.WriteLine("[CurrentUserService] All Claims:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"  - Type: {claim.Type}, Value: {claim.Value}");
            }
        }
        
        // Try different claim types - JWT claims can be mapped to different types
        var userIdClaim = user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user?.FindFirst("sub")?.Value;
            
        Console.WriteLine($"[CurrentUserService] UserId Claim: {userIdClaim}");
        
        if (string.IsNullOrEmpty(userIdClaim))
            return null;
        
        return long.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }

    public string? GetUsername()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}
