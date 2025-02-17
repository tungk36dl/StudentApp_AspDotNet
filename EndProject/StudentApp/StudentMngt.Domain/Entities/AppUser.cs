using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public bool IsSystemUser { get; set; }

        public string Code { get; set; }

        public string? FullName { get; set; }

        public Guid? ClassesId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
