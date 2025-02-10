using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public class UpdateScoreViewModel
    {
        [Required]
        public Double ScoreValue { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
