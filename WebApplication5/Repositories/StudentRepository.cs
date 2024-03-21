using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using System.Data.Common;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>>SearchStudentsAsync (string searchString)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.Contains(searchString));
            }

            return await query.ToListAsync();
        }

        public async Task<List<CourseEnrollment>> GetCourseEnrollmentsByStudentIdAsync(int studentId)
        {
            var courseEnrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => new CourseEnrollment
                {
                    CourseId = e.Course.CourseId,
                CourseName = e.Course.CourseName,
                CourseUnit = e.Course.CourseUnit,
                ProzessGrade = e.ProzessGrade,
                ComponentGrade = e.ComponentGrade,
                })
                .ToListAsync();

            return courseEnrollments;
        }

    }
}
