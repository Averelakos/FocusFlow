using Microsoft.AspNetCore.Mvc;

public class AuthorizeJwtAttribute : TypeFilterAttribute
    {
        public AuthorizeJwtAttribute() : base(typeof(AuthorizeJwtFilter))
        {
            Arguments = new object[] { };
        }
    }