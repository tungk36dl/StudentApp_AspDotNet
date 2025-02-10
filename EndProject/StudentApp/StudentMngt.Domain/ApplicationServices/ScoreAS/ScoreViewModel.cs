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
        public Double ScoreValue { get; set; } 
        public Guid UserId { get; set; }
        public Guid SubjectDetailId { get; set; }
        public Semesters Semesters {  get; set; } 
        public EntityStatus Status { get; set; }
    }
}
