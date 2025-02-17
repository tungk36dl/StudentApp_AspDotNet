using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.MajorAS
{
    public class MajorViewModel
    {
        public Guid Id { get; set; }
        public String MajorName { get; set; } 
        public EntityStatus Status { get; set; }
    }
}
