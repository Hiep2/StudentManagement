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
        try
        {
            return await _studentRepository.GetAllStudentsAsync();
        }
        catch(Exception ex)
        {
            throw new ServiceException("An error occurred while retrieving all students.", ex);
        }

    }

    public async Task<Student> GetStudentByIdAsync(int studentId)
    {
        try
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new NotFoundException($"Student with ID {studentId} not found.");
            }
            return student;
        }
        catch (Exception ex)
        {
            throw new ServiceException($"An error occurred while retrieving student with ID {studentId}.", ex);
        }   
    }

    public async Task<Student> AddStudentAsync(Student student)
    {
        try
        {
            if (student == null)
            {
                throw new NotFoundException($"Student with ID {student.StudentId} not found.");
            }
            return await _studentRepository.AddStudentAsync(student);
        }
        catch (Exception ex)
        {
            throw new ServiceException($"An error occurred while retrieving student with ID {student.StudentId}.", ex);
        }
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        try
        {
            if (student == null)
            {
                throw new NotFoundException($"Student with ID {student.StudentId} not found.");
            }
            return await _studentRepository.UpdateStudentAsync(student); ;
        }
        catch (Exception ex)
        {
            throw new ServiceException($"An error occurred while retrieving student with ID {student.StudentId}.", ex);
        }
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        try
        {
            await _studentRepository.DeleteStudentAsync(studentId);
        }
        catch (Exception ex)
        {
            throw new ServiceException($"An error occurred while retrieving student with ID {studentId}.", ex);
        }
    }

    public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchString)
    {
        return await _studentRepository.SearchStudentsAsync(searchString);
    }

    public async Task<List<CourseEnrollment>> GetCourseEnrollmentsByStudentIdAsync(int studentId)
    {
        return await _studentRepository.GetCourseEnrollmentsByStudentIdAsync(studentId);
    }

    public async Task UpdateCourseEnrollmentAsync(CourseEnrollment courseEnrollment)
    {
        await _courseRepository.UpdateCourseEnrollmentAsync(courseEnrollment);
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
        return await _courseRepository.GetCourseEnrollmentByStudentAsync(studentId, courseId);
    }

    public async Task AddCourseEnrollmentAsync(int studentId, CourseEnrollment courseEnrollment)
    {
        await _courseRepository.AddCourseEnrollmentAsync(studentId, courseEnrollment);
    }

    public async Task AssignCourseToStudentAsync(CourseEnrollment courseEnrollment)
    {
        await _courseRepository.AssignCourseToStudentAsync(courseEnrollment);
    }
}
