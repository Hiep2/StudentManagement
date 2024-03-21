using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Mysqlx.Crud;
using System.Data;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolContext _context;

        public CourseRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }

        public async Task UpdateCourseEnrollmentAsync(CourseEnrollment courseEnrollment)
        {
            var course = await _context.Courses.FindAsync(courseEnrollment.CourseId);
            if (course != null)
            {
                // Kurs aktualisieren
                course.CourseName = courseEnrollment.CourseName;
                course.CourseUnit = courseEnrollment.CourseUnit;
                _context.Courses.Update(course);
            }

            var enrollment = await _context.Enrollments
                             .FirstOrDefaultAsync(e => e.CourseId == courseEnrollment.CourseId && e.StudentId == courseEnrollment.StudentId);
            if (enrollment != null)
            {
                // Einschreibung aktualisieren
                enrollment.ProzessGrade = courseEnrollment.ProzessGrade;
                enrollment.ComponentGrade = courseEnrollment.ComponentGrade;
                _context.Enrollments.Update(enrollment);
            }

            // Die Änderungen in einer Transaktion speichern
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public async Task DeleteCourseEnrollmentAsync(int courseId, int studentId)
        {
            var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);               
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CourseEnrollment> GetCourseEnrollmentByStudentAsync(int studentId, int courseId)
        {
            var courseEnrollment = await _context.Enrollments
                .Where(e => e.CourseId == courseId && e.StudentId == studentId)
                .Select(e => new CourseEnrollment
                {
                    CourseId = e.CourseId,
                    StudentId = e.StudentId,
                    CourseName = e.Course.CourseName,
                    CourseUnit = e.Course.CourseUnit,
                    ProzessGrade = e.ProzessGrade,
                    ComponentGrade = e.ComponentGrade
                })
                .FirstOrDefaultAsync();

            return courseEnrollment;
        }

        public async Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseEnrollment)
        {
            var course = new Course
            {
                CourseName = courseEnrollment.CourseName,
                CourseUnit = courseEnrollment.CourseUnit
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = course.CourseId,
                ProzessGrade = courseEnrollment.ProzessGrade,
                ComponentGrade = courseEnrollment.ComponentGrade
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task AssignCourseToStudentAsync(CourseEnrollment courseEnrollment)
        {
            var enrollment = new Enrollment
            {
                StudentId = courseEnrollment.StudentId,
                CourseId = courseEnrollment.CourseId,
                ProzessGrade = 0,
                ComponentGrade = 0
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

    }
}
