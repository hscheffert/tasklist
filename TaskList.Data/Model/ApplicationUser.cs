using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskList.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminRole { get; set; }
        public bool IsSupervisor { get; set; }
        public string Role { get; set; }
    }
}
