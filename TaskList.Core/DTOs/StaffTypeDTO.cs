using System;

namespace TaskList.Core.DTOs
{
    public partial class StaffTypeDTO
    {
        public StaffTypeDTO()
        {
        }

        public StaffTypeDTO(Guid id)
            : this()
        {
            this.StaffTypeId = id;
        }

        public Guid StaffTypeId { get; set; }
        public string Name { get; set; }
        public bool AllowMultiple { get; set; }
        public bool IsSupervisor { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // public IEnumerable<StaffDTO> Staff { get; set; }
    }
}
