using Microsoft.AspNetCore.Authorization;

namespace TaskList.Web
{
    public class HasClaimRequirement : IAuthorizationRequirement
    {
        public readonly string ClaimType;

        public HasClaimRequirement(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
