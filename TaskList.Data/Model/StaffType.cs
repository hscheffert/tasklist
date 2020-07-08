using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class StaffType
    {
        public StaffType()
        {
            Staff = new HashSet<Staff>();
        }

        [Key]
        [Column("StaffTypeID")]
        public Guid StaffTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool AllowMultiple { get; set; }
        public bool IsSupervisor { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("StaffType")]
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
