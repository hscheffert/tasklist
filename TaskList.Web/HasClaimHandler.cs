using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Web
{
    public class HasClaimHandler : AuthorizationHandler<HasClaimRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, HasClaimRequirement requirement)
        {
            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.ClaimType);           
            if(roleClaim != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
