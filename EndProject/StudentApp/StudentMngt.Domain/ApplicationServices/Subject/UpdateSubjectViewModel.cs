using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.Subject
{
    public class UpdateSubjectViewModel
    {
        [Required]
        public String SubjectName { get; set; }

        [Required]
        public Guid SubjectId { get; set; }
    }
}
