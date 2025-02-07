using System.ComponentModel.DataAnnotations;

namespace StudentMngt.Domain.ApplicationServices.CohortAS
{
    public class CreateCohortViewModel
    {
        [Required]
        public String CohortName { get; set; }
    }
}
