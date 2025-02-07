using Microsoft.AspNetCore.Mvc;
using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    [Table("SubjectDetails")]
    public class SubjectDetail : DomainEntity<Guid>, IAuditTable
    {
        public int Credits { get; set; }

        public Guid TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public AppUser Teacher { get; set; }

        public Guid SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        public Guid ClassId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public EntityStatus Status { get; set; }

    }
}
