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
        public Double Score { get; set; }

    }
}
