using System.ComponentModel.DataAnnotations;

namespace StudentMngt.Domain.ApplicationServices.ClassesAS
{
    public class CreateClassesViewModel
    {
        [Required]
        public String ClassesName { get; set; }

        [Required]
        public Guid MajorId { get; set; }
    }
}
