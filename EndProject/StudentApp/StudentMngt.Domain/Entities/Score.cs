using StudentMngt.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMngt.Domain.Entities
{
    [Table("Scores")]
    public class Score : DomainEntity<Guid>, IAuditTable
	{
        public Guid SubjectDetailId { get; set; }
        public Guid UserId { get; set; }

        public Double? AttendanceScore { get; set; }
        public Double? TestScore { get; set; }
        public Double? FinalScore { get; set; }
        public Double? GPA { get; set; }
        public LetterGrades?  LetterGrades { get; set; }



        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}
