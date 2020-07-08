using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Data.Model
{
    public partial class User
    {
        public User()
        {
            Staff = new HashSet<Staff>();
        }

        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public bool IsSupervisor { get; set; }
        [Column("SupervisorID")]
        public Guid? SupervisorId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
