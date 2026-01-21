using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// Authorization filter that validates JWT tokens from the Authorization header
/// </summary>
public class AuthorizeJwtFilter : IAuthorizationFilter
{
    private readonly ITokenProviderService _tokenProviderService;

    public AuthorizeJwtFilter(ITokenProviderService tokenProviderService)
    {
        _tokenProviderService = tokenProviderService;
        // _requiredPermissions = requiredPermissions?.ToList() ?? new List<EPermission>();
    }

    /// <summary>
    /// Validates JWT token from Authorization header and sets user principal in context
    /// </summary>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string token;
        string? authHeader = context.HttpContext.Request.Headers["Authorization"];

        Console.WriteLine($"[AuthorizeJwtFilter] Authorization Header: {authHeader}");

        if ( authHeader != null && authHeader.StartsWith("Bearer ") )
        {
            token = authHeader[7..];
            Console.WriteLine($"[AuthorizeJwtFilter] Token extracted: {token[..20]}...");
            
            var userPrincipal = _tokenProviderService.ValidateToken(token);

            if (userPrincipal is not null)
            {
                context.HttpContext.User = userPrincipal;
                Console.WriteLine($"[AuthorizeJwtFilter] User authenticated: {userPrincipal.Identity?.Name}");
            }
            else
            {
                Console.WriteLine("[AuthorizeJwtFilter] Token validation failed");
                // bug : ForbidResult returns 500
                context.Result = new StatusCodeResult(403);
            }
        }
        else
        {
            Console.WriteLine("[AuthorizeJwtFilter] No Bearer token found");
            context.Result = new UnauthorizedResult();
        }
    }
}