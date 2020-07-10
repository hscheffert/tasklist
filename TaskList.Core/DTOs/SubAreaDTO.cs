using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Core.DTOs
{
    public partial class SubAreaDTO
    {
        public SubAreaDTO()
        {
        }

        public SubAreaDTO(Guid id)
            : this()
        {
            this.SubAreaId = id;
        }

        public Guid SubAreaId { get; set; }
        public Guid AreaId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AreaName { get; set; }
    }
}
