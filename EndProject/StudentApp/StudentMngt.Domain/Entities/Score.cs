using StudentMngt.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMngt.Domain.Entities
{
    [Table("Scores")]
    public class Score : DomainEntity<Guid>, IAuditTable
	{
        public Guid SubjectDetailId { get; set; }
        public Guid UserId { get; set; }

        public Double ScoreValue { get; set; }
        public Semesters Semesters { get; set; }

        public TypeScore TypeScore { get; set; }


        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}
