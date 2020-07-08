using System;
using System.Collections.Generic;

namespace TaskList.Core.DTOs
{
    public partial class UserDTO
    {
        public UserDTO()
        {
        }

        public UserDTO(Guid id)
            : this()
        {
            this.UserId = id;
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSupervisor { get; set; }
        public Guid? SupervisorId { get; set; }
        public bool IsActive { get; set; }
        public string SupervisorName { get; set; }
    }
}
