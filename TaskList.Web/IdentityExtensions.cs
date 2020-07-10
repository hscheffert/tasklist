using System;
using System.Security.Claims;

namespace TaskList.Web
{
    public static class IdentityExtensions
    {
        public static string GetCurrentUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
