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
        Task<List<CourseDetail>> GetCourseDetailsByStudentIdAsync(int studentId);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
        Task UpdateCourseEnrollmentAsync(CourseDetail courseDetail);
        Task DeleteCourseEnrollmentAsync(int courseId, int studentId);
        Task<CourseDetail> GetCourseDetailByStudentAsync(int studentId, int courseId);
        Task AddCourseDetailAsync(int studentId, CourseDetail courseDetail);
        Task AssignCourseToStudentAsync(CourseDetail courseDetail);
    }
}
