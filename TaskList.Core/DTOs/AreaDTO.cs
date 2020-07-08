using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Core.DTOs
{
    public partial class AreaDTO
    {
        public AreaDTO()
        {
        }

        public AreaDTO(Guid id)
            : this()
        {
            this.AreaId = id;
        }

        public Guid AreaId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public IEnumerable<SubAreaDTO> SubAreas { get; set; }
    }
}
