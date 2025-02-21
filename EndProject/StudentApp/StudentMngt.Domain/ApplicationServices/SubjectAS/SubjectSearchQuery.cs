using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectAS
{
    public class SubjectSearchQuery : SearchQuery
    {
        public Guid? UserId { get; set; }
        public Guid Id { get; set; }
    }
}
