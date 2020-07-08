using System;
using System.Collections.Generic;

namespace TaskList.Core.DTOs
{
    public partial class FrequencyDTO
    {
        public FrequencyDTO()
        {
        }

        public FrequencyDTO(Guid id)
            : this()
        {
            this.FrequencyId = id;
        }

        public Guid FrequencyId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // public IEnumerable<TaskDTO> Task { get; set; }
    }
}
