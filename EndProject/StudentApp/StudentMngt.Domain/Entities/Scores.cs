using StudentMngt.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMngt.Domain.Entities
{
    [Table("Scores")]
    public class Scores : DomainEntity<Guid>, IAuditTable
	{
        public Guid SubjectDetailId { get; set; }
        public Guid UserId { get; set; }

        public Double Score { get; set; }
        public Semesters Semesters { get; set; }


        public DateTime? CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EntityStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
