using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class Frequency
    {
        public Frequency()
        {
            Task = new HashSet<Task>();
        }

        [Key]
        [Column("FrequencyID")]
        public Guid FrequencyId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("Frequency")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
