using System;

namespace TaskList.Core.DTOs
{
    public partial class TaskStaffDTO
    {
        public Guid? UserId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? StaffId { get; set; }
        public Guid StaffTypeId { get; set; }
        public string StaffTypeName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
       
    }
}
