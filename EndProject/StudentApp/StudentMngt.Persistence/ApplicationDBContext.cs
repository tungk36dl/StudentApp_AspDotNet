using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentMngt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Persistence
{
    public class ApplicationDBContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<SubjectDetail> SubjectDetails { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Cohort> Cohorts { get; set; }
        public DbSet<Major> Majors { get; set; }

    }
}
