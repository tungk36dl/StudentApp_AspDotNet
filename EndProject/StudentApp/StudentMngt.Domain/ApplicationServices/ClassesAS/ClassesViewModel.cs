using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ClassesAS
{
    public class ClassesViewModel
    {
        public Guid Id { get; set; }
        public String ClassesName { get; set; } 
        public Guid MajorId     { get; set; }
        public Guid CohortId { get; set; }
        public EntityStatus Status { get; set; }
    }
}
