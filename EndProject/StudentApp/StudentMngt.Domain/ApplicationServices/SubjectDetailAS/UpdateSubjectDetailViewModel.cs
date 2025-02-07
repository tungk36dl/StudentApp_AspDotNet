using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectDetailAS
{
    public class UpdateSubjectDetailViewModel
    {
        public Guid SubjectDetailId { get; set; }
        public int Credits { get; set; }

    }
}
