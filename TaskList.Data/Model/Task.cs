using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class Task
    {
        public Task()
        {
            Staff = new HashSet<Staff>();
        }

        [Key]
        [Column("TaskID")]
        public Guid TaskId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Column("AreaID")]
        public Guid AreaId { get; set; }
        [Column("SubAreaID")]
        public Guid SubAreaId { get; set; }
        [Column("FrequencyID")]
        public Guid FrequencyId { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public bool IsInPolicyTech { get; set; }
        [StringLength(150)]
        public string ProcedureFileName { get; set; }
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
        [InverseProperty("Task")]
        public virtual Area Area { get; set; }
        [ForeignKey(nameof(FrequencyId))]
        [InverseProperty("Task")]
        public virtual Frequency Frequency { get; set; }
        [ForeignKey(nameof(SubAreaId))]
        [InverseProperty("Task")]
        public virtual SubArea SubArea { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
