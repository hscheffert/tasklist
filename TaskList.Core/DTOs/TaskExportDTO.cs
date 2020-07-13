using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace TaskList.Core.DTOs
{
    public partial class TaskExportDTO
    {
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public string FrequencyName { get; set; }
        public string Notes { get; set; }
        public bool IsInPolicyTech { get; set; }
        public string ProcedureFileName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string PrimaryStaffName { get; set; }
        public IEnumerable<TaskStaffDTO> SecondaryStaff { get; set; }
        //public IEnumerable<TaskStaffDTO> Staff { get; set; }
        ///public IEnumerable<StaffGroupDTO> SecondaryStaff { get; set; }
        //public ILookup<string, TaskStaffDTO> SecondaryStaff { get; set; }
    }
}
