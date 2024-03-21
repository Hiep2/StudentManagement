using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseEnrollmentRepository _courseRepository;

    public StudentService(IStudentRepository studentRepository, ICourseEnrollmentRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllStudentsAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int studentId)
    {
        return await _studentRepository.GetStudentByIdAsync(studentId);
    }

    public async Task<Student> AddStudentAsync(Student student)
    {
        return await _studentRepository.AddStudentAsync(student);
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        return await _studentRepository.UpdateStudentAsync(student);
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        await _studentRepository.DeleteStudentAsync(studentId);
    }

    public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchString)
    {
        return await _studentRepository.SearchStudentsAsync(searchString);
    }

    public async Task<List<CourseEnrollment>> GetCourseEnrollmentByStudentIdAsync(int studentId)
    {
        return await _studentRepository.GetCourseEnrollmentByStudentIdAsync(studentId);
    }

    public async Task UpdateCourseEnrollmentAsync(CourseEnrollment courseDetail)
    {
        await _courseRepository.UpdateCourseEnrollmentAsync(courseDetail);
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _courseRepository.GetAllStudentCoursesAsync();
    }

    public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
    {
        return await _courseRepository.GetAllStudentEnrollmentsAsync();
    }

    public async Task DeleteCourseEnrollmentAsync(int courseId, int studentId)
    {
        await _courseRepository.DeleteCourseEnrollmentAsync(courseId, studentId);
    }

    public async Task<CourseEnrollment> GetCourseEnrollmentByStudentAsync(int studentId, int courseId)
    {
        return await _courseRepository.GetCourseEnrollmentFromStudentAsync(studentId, courseId);
    }

    public async Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseDetail)
    {
        await _courseRepository.AddCourseEnrollmentToStudentAsync(studentId, courseDetail);
    }

    public async Task AssignCourseEnrollmentToStudentAsync(CourseEnrollment courseDetail)
    {
        await _courseRepository.AssignCourseEnrollmentToStudentAsync(courseDetail);
    }
}
