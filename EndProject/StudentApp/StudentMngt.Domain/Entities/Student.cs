using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    [Table("Students")]
    public class Student : DomainEntity<Guid>, IAuditTable
    {
        [Column(TypeName ="nvarchar(1000)")]
        public String StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public String Address { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public Guid ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Classes Classes { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public EntityStatus Status { get; set; }
    }
}
