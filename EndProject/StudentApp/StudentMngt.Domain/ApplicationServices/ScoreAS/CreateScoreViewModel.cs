using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public class CreateScoreViewModel
    {
        [Required]
        public Double ScoreValue { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid SubjectDetailId { get; set; }

        [Required]
        public TypeScore TypeScore { get; set; }


    }
}
