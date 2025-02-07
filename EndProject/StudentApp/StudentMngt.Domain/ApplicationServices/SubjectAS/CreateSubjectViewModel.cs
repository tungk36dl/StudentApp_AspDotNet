using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectAS
{
    public class CreateSubjectViewModel
    {
        [Required]
        public String SubjectName { get; set; }
        

    }
}
