using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
    }
}
