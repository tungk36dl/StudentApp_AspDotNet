using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    [Table("TeachersSubjects")]
    public class TeacherSubject : DomainEntity<Guid>
    {
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; } = Guid.Empty;

    }
}
