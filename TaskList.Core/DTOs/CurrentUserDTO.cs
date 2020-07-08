using System;
using System.Collections.Generic;

namespace TaskList.Core.DTOs
{
    public partial class CurrentUserDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
