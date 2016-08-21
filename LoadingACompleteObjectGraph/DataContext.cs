using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadingACompleteObjectGraph
{
    public class DataContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id)
                .HasMany(c => c.Sections)
                .WithRequired(c => c.Course)
                .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.Id)
                .HasMany(s => s.Students)
                .WithMany(s => s.Sections)
                .Map(s =>
                {
                    s.MapLeftKey("SectionId");
                    s.MapRightKey("StudentId");
                    s.ToTable("SectionStudents");
                });

            modelBuilder.Entity<Section>()
                .HasRequired(s => s.Instructor)
                .WithMany(s => s.Sections)
                .HasForeignKey(s => s.InstructorId);
        }
    }
}
