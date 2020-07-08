using Microsoft.AspNetCore.Authorization;

namespace TaskList.Web
{
    public class IsAdminRequirement : IAuthorizationRequirement
    {
        public readonly string Role;

        public IsAdminRequirement(string role)
        {
            Role = role;
        }
    }
}
