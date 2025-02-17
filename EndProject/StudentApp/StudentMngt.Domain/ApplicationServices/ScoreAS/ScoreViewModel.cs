using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public class ScoreViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid SubjectDetailId { get; set; }


        public Double? AttendanceScore { get; set; }
        public Double? TestScore { get; set; }
        public Double? FinalScore { get; set; }
        public Double? GPA { get; set; }
        public LetterGrades? LetterGrades { get; set; }


        public EntityStatus Status { get; set; }
    }
}
