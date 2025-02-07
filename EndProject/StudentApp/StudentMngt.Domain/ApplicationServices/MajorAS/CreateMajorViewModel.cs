using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.MajorAS
{
    public class CreateMajorViewModel
    {
        [Required]
        public String MajorName { get; set; }

        [Required]
        public Guid CohortId { get; set; }
    }
}
