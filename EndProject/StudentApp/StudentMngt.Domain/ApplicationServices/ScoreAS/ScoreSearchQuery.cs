using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public class ScoreSearchQuery : SearchQuery
    {
        public Guid UserId { get; set; }
        public Guid SubjectDetailId { get; set; }
    }
}
