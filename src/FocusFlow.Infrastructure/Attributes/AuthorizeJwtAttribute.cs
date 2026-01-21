using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Attribute to apply JWT authentication authorization to controllers or actions
/// </summary>
public class AuthorizeJwtAttribute : TypeFilterAttribute
    {
        public AuthorizeJwtAttribute() : base(typeof(AuthorizeJwtFilter))
        {
            Arguments = new object[] { };
        }
    }