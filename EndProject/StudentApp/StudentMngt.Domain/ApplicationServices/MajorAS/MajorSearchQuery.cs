﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.MajorAS
{
    public class MajorSearchQuery : SearchQuery
    {
        public Guid? UserId { get; set; }
        public Guid Id { get; set; }
    }
}
