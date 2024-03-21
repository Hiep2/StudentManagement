using WebApplication5.Models;

namespace WebApplication5.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int studentId);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchString);
        Task<List<CourseEnrollment>> GetCourseEnrollmentByStudentIdAsync(int studentId);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
        Task UpdateCourseEnrollmentAsync(CourseEnrollment courseEnrollment);
        Task DeleteCourseEnrollmentAsync(int courseId, int studentId);
        Task<CourseEnrollment> GetCourseEnrollmentByStudentAsync(int studentId, int courseId);
        Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseEnrollment);
        Task AssignCourseEnrollmentToStudentAsync(CourseEnrollment courseEnrollment);
    }
}
