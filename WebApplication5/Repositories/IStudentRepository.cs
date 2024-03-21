using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication5.Models;
public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student> GetStudentByIdAsync(int studentId);
    Task<Student> AddStudentAsync(Student student);
    Task<Student> UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int studentId);
    Task<IEnumerable<Student>> SearchStudentsAsync(string searchString);
    Task<List<CourseEnrollment>> GetCourseEnrollmentsByStudentIdAsync(int studentId);
}
