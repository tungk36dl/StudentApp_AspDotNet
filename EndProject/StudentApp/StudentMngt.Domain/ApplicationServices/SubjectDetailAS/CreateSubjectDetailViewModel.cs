using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectDetailAS
{
    public class CreateSubjectDetailViewModel
    {
        public int Credits { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
