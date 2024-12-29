using StudentMngt.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMngt.Domain.Entities
{
    [Table("Score")]
    public class Score : DomainEntity<Guid>
	{
        public Guid SubjectDetailId { get; set; }
        public String StudentCode { get; set; }
        public Decimal score {  get; set; }

        [ForeignKey(nameof(StudentCode))]
        public AppUser Student { get; set; }

        [ForeignKey(nameof(SubjectDetailId))]
        public SubjectDetail SubjectDetail { get; set; }

        public Semesters Semesters { get; set; }

    }
}
