using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.CohortAS
{
    public class UpdateCohortViewModel
    {
        [Required]
        public String CohortName { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
