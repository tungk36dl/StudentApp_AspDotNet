using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    [Table("Cohorts")]
    public class Cohort : DomainEntity<Guid>, IAuditTable
    {
        public String CohortName { get; set; }
        public DateTime? CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EntityStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
