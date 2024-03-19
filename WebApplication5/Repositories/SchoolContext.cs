using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public SchoolContext(DbContextOptions<SchoolContext> options)
                : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=schooldb;user=root;password=yourpassword",
                    new MariaDbServerVersion(new Version(10, 4, 14))); // Ändern Sie die Version entsprechend Ihrer MariaDB/MySQL-Version
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Beispiel: Eine Entität explizit als keyless markieren
            modelBuilder.Entity<CourseDetail>().HasKey(c => c.CourseId);

            // Beispiel: Primärschlüssel für eine Entität definieren
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);    
        }

    }
}
