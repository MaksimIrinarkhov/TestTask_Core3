using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask_Core3.Entities;

namespace TestTask_Core3.Repository
{
    public class TestDBContext : DbContext
    {
        public TestDBContext(DbContextOptions<TestDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentsGroup>()
               .HasKey(pc => new { pc.StudentId, pc.GroupId });
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<StudentsGroup> StudentsGroups { get; set; }
    }
}
