using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
        Task UpdateCourseEnrollmentAsync(CourseEnrollment courseDetail);
        Task DeleteCourseEnrollmentAsync(int courseId, int studentId);
        Task<CourseEnrollment> GetCourseEnrollmentByStudentAsync(int studentId, int courseId);
        Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseDetail);
        Task AssignCourseToStudentAsync(CourseEnrollment courseDetail);
    }
}
