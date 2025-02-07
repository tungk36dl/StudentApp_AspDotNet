using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ClassesAS
{
    public class ClassesSearchQuery : SearchQuery
    {
        public Guid UserId { get; set; }
    }
}
