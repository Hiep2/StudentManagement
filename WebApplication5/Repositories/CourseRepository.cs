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

        public async Task UpdateCourseEnrollmentAsync(CourseDetail courseDetail)
        {
            var course = await _context.Courses.FindAsync(courseDetail.CourseId);
            if (course != null)
            {
                // Kurs aktualisieren
                course.CourseName = courseDetail.CourseName;
                course.CourseUnit = courseDetail.CourseUnit;
                _context.Courses.Update(course);
            }

            var enrollment = await _context.Enrollments
                             .FirstOrDefaultAsync(e => e.CourseId == courseDetail.CourseId && e.StudentId == courseDetail.StudentId);
            if (enrollment != null)
            {
                // Einschreibung aktualisieren
                enrollment.ProzessGrade = courseDetail.ProzessGrade;
                enrollment.ComponentGrade = courseDetail.ComponentGrade;
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

        public async Task<CourseDetail> GetCourseDetailByStudentAsync(int studentId, int courseId)
        {
            var courseDetail = await _context.Enrollments
                .Where(e => e.CourseId == courseId && e.StudentId == studentId)
                .Select(e => new CourseDetail
                {
                    CourseId = e.CourseId,
                    StudentId = e.StudentId,
                    CourseName = e.Course.CourseName,
                    CourseUnit = e.Course.CourseUnit,
                    ProzessGrade = e.ProzessGrade,
                    ComponentGrade = e.ComponentGrade
                })
                .FirstOrDefaultAsync();

            return courseDetail;
        }

        public async Task AddCourseDetailAsync(int studentId, CourseDetail courseDetail)
        {
            var course = new Course
            {
                CourseName = courseDetail.CourseName,
                CourseUnit = courseDetail.CourseUnit
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = course.CourseId,
                ProzessGrade = courseDetail.ProzessGrade,
                ComponentGrade = courseDetail.ComponentGrade
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task AssignCourseToStudentAsync(CourseDetail courseDetail)
        {
            var enrollment = new Enrollment
            {
                StudentId = courseDetail.StudentId,
                CourseId = courseDetail.CourseId,
                ProzessGrade = 0,
                ComponentGrade = 0
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

    }
}
