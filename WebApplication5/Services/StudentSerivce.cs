﻿using WebApplication5.IRepositories;
using WebApplication5.Models;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseEnrollmentRepository _courseEnrollmentRepository;

    public StudentService(IStudentRepository studentRepository, ICourseEnrollmentRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseEnrollmentRepository = courseRepository;
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
        await _courseEnrollmentRepository.UpdateCourseEnrollmentAsync(courseEnrollment);
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _courseEnrollmentRepository.GetAllStudentCoursesAsync();
    }

    public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
    {
        return await _courseEnrollmentRepository.GetAllStudentEnrollmentsAsync();
    }

    public async Task DeleteCourseEnrollmentAsync(int courseId, int studentId)
    {
        await _courseEnrollmentRepository.DeleteCourseEnrollmentAsync(courseId, studentId);
    }

    public async Task<CourseEnrollment> GetCourseEnrollmentFromStudentAsync(int studentId, int courseId)
    {
        return await _courseEnrollmentRepository.GetCourseEnrollmentFromStudentAsync(studentId, courseId);
    }

    public async Task AddCourseEnrollmentToStudentAsync(int studentId, CourseEnrollment courseEnrollment)
    {
        await _courseEnrollmentRepository.AddCourseEnrollmentToStudentAsync(studentId, courseEnrollment);
    }

    public async Task AssignCourseEnrollmentToStudentAsync(CourseEnrollment courseEnrollment)
    {
        await _courseEnrollmentRepository.AssignCourseEnrollmentToStudentAsync(courseEnrollment);
    }
}
