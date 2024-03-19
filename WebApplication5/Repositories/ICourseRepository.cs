using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
        Task UpdateCourseEnrollmentAsync(CourseDetail courseDetail);
        Task DeleteCourseEnrollmentAsync(int courseId, int studentId);
        Task<CourseDetail> GetCourseDetailByStudentAsync(int studentId, int courseId);
        Task AddCourseDetailAsync(int studentId, CourseDetail courseDetail);
        Task AssignCourseToStudentAsync(CourseDetail courseDetail);
    }
}
