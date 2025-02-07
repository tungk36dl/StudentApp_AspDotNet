using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectDetailAS
{
    public class SubjectDetailViewModel
    {
        public Guid SubjectDetailId { get; set; }
        public int Credits { get; set; }
        public String SubjectName { get; set; }
        public Subject Subject { get; set; }
        public AppUser Teacher { get; set; }
        public String TeacherName { get; set; }

        public EntityStatus Status { get; set; }

    }
}
