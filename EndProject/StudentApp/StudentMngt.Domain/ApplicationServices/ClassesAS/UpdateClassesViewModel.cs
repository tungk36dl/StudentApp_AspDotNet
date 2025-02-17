using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ClassesAS
{
    public class UpdateClassesViewModel
    {
        [Required]
        public String ClassesName { get; set; }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid MajorId { get; set; }

        [Required]
        public Guid CohortId { get; set; }
    }
}
