using System;

namespace TaskList.Core.DTOs
{
    public partial class StaffMemberDTO
    {
        public StaffMemberDTO()
        {
        }

        public StaffMemberDTO(Guid id)
            : this()
        {
            this.StaffTypeId = id;
        }

        public Guid StaffTypeId { get; set; }
        public string StaffTypeName { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSupervisor { get; set; }
    }
}
