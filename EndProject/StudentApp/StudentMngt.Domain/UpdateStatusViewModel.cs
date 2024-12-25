using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain
{
    internal class UpdateStatusViewModel
    {
        public EntityStatus Status { get; set; }

        public Guid Id { get; set; }
    }
}
