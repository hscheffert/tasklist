using Microsoft.AspNetCore.Identity;

namespace TaskList.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminRole { get; set; }
    }
}
