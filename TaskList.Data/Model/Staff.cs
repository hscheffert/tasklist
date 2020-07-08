using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class Staff
    {
        [Key]
        [Column("StaffID")]
        public Guid StaffId { get; set; }
        [Column("StaffTypeID")]
        public Guid StaffTypeId { get; set; }
        [Column("TaskID")]
        public Guid TaskId { get; set; }
        [Column("UserID")]
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(StaffTypeId))]
        [InverseProperty("Staff")]
        public virtual StaffType StaffType { get; set; }
        [ForeignKey(nameof(TaskId))]
        [InverseProperty("Staff")]
        public virtual Task Task { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Staff")]
        public virtual User User { get; set; }
    }
}
