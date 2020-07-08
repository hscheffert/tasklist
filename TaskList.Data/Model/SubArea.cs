using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class SubArea
    {
        public SubArea()
        {
            Task = new HashSet<Task>();
        }

        [Key]
        [Column("SubAreaID")]
        public Guid SubAreaId { get; set; }
        [Column("AreaID")]
        public Guid AreaId { get; set; }
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

        [ForeignKey(nameof(AreaId))]
        [InverseProperty("SubArea")]
        public virtual Area Area { get; set; }
        [InverseProperty("SubArea")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
