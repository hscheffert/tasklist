using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Web
{
    public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            var claims = context.User.Claims.ToList();
            var isAdminClaim = context.User.Claims
                 .FirstOrDefault(claim => claim.Type == "AdminRole" &&
                     claim.Value.Equals(requirement.Role, StringComparison.InvariantCultureIgnoreCase));

            if (isAdminClaim != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
