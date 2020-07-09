using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace TaskList.Web
{
    public static class IdentityExtensions
    {
        public static void RemoveClaim(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;

            if (identity == null)
                return;

            // Check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);

            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);
        }
    }
}
