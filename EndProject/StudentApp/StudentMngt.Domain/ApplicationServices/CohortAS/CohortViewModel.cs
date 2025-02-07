using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.CohortAS
{
    public class CohortViewModel
    {
        public Guid Id { get; set; }
        public String CohortName { get; set; } 
        public EntityStatus Status { get; set; }
    }
}
