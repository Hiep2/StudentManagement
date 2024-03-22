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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await UpdateCourseAsync(courseEnrollment.CourseId, courseEnrollment.CourseName, courseEnrollment.CourseUnit);
                    await UpdateEnrollmentGradesAsync(courseEnrollment.CourseId, courseEnrollment.StudentId, courseEnrollment.ProzessGrade, courseEnrollment.ComponentGrade);
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

        private async Task UpdateCourseAsync(int courseId, string newName, int newUnits)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course != null)
            {
                course = new Course(newName, newUnits);
                _context.Courses.Update(course);
            }
        }

        private async Task UpdateEnrollmentGradesAsync(int courseId, int studentId, double newProzessGrade, double newComponentGrade)
        {
            var enrollment = await _context.Enrollments
                                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);
            if (enrollment != null)
            {
                enrollment.SetProzessGrade(newProzessGrade);
                enrollment.SetComponentGrade(newComponentGrade);
                _context.Enrollments.Update(enrollment);
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
                .Select(e => new CourseEnrollment(e.CourseId, e.Course.CourseName, e.Course.CourseUnit, e.ProzessGrade, e.ComponentGrade, studentId))
                .FirstOrDefaultAsync();

            if (courseEnrollment == null)
            {
                throw new KeyNotFoundException($"Course with the ID {courseId} was not found in the Student with the ID {studentId}.");
            }

            return courseEnrollment;
        }

        public async Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseEnrollment)
        {
            var course = new Course(courseEnrollment.CourseName, courseEnrollment.CourseUnit);
            _context.Courses.Add(course);

            var enrollment = new Enrollment(course.CourseId, studentId, courseEnrollment.ProzessGrade, courseEnrollment.ComponentGrade);
            _context.Enrollments.Add(enrollment);

            await _context.SaveChangesAsync();
        }

        public async Task AssignCourseToStudentAsync(CourseEnrollment courseEnrollment)
        {
            var enrollment = new Enrollment(courseEnrollment.CourseId, courseEnrollment.StudentId, courseEnrollment.ProzessGrade, courseEnrollment.ComponentGrade); 

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

    }
}
