using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace TaskList.Core.DTOs
{
    public partial class TaskDTO
    {
        public TaskDTO()
        {
        }

        public TaskDTO(Guid id)
            : this()
        {
            this.TaskId = id;
        }

        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public Guid AreaId { get; set; }
        public string AreaName { get; set; }
        public Guid SubAreaId { get; set; }
        public string SubAreaName { get; set; }
        public Guid FrequencyId { get; set; }
        public string FrequencyName { get; set; }
        public string Notes { get; set; }
        public bool IsInPolicyTech { get; set; }
        public string ProcedureFileName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PrimaryStaffName { get; set; }
        public Guid RowKey { get; set; }
    }
}
