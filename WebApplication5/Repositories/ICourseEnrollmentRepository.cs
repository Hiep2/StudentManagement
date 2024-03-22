using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public interface ICourseEnrollmentRepository
    {
        Task<List<Course>> GetAllStudentCoursesAsync();
        Task<List<Enrollment>> GetAllStudentEnrollmentsAsync();
        Task UpdateCourseEnrollmentAsync(CourseEnrollment courseEnrollment);
        Task DeleteCourseEnrollmentAsync(int courseId, int studentId);
        Task<CourseEnrollment> GetCourseEnrollmentFromStudentAsync(int studentId, int courseId);
        Task AddCourseEnrollmentToStudentAsync(int studentId, CourseEnrollment courseEnrollment);
        Task AssignCourseEnrollmentToStudentAsync(CourseEnrollment courseEnrollment);
    }
}