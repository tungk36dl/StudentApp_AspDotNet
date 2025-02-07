using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectAS
{
    public class SubjectViewModel
    {
        public Guid Id { get; set; }
        public String SubjectName { get; set; } 
        public EntityStatus Status { get; set; }
    }
}
