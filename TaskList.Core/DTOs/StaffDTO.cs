using System;
using System.Collections.Generic;

namespace TaskList.Core.DTOs
{
    public partial class StaffDTO
    {
        public StaffDTO()
        {
        }

        public StaffDTO(Guid id)
            : this()
        {
            this.StaffId = id;
        }

        public Guid StaffId { get; set; }
        public Guid StaffTypeId { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TaskName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffTypeName { get; set; }
        public bool IsSupervisor { get; set; }
    }
}
