﻿using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApplication5.IRepositories;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            List<Student> students;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                students = (await studentService.GetAllStudentsAsync()).ToList();
            }
            else
            {
                students = (await studentService.SearchStudentsAsync(searchString)).ToList();
            }

            return View(new StudentViewModel()
            {
                Students = students
            });
        }

        public ActionResult Create()
        {
            var student = new Student();
            return View(ViewNames.CreateEditPartial, student);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var student = await studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmAsync(int id)
        {
            await studentService.DeleteStudentAsync(id);
            return RedirectToAction(ViewNames.Index);
        }

        public async Task<ActionResult> EditAsync(int id)
        {
            var student = await studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(ViewNames.CreateEditPartial, student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAsync(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.StudentId == 0)
                {
                    await studentService.AddStudentAsync(student);
                }
                else
                {
                    await studentService.UpdateStudentAsync(student);
                }

                return RedirectToAction(ViewNames.Index);
            }

            return View(ViewNames.CreateEditPartial, student);
        }

        public async Task<ActionResult> Details(int id)
        {
            var student = await studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var courseEnrollment = await studentService.GetCourseEnrollmentsByStudentIdAsync(id);

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                _CoursesEnrollments = courseEnrollment
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ImportCSV(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    bool isFirstLine = true;
                    while (!stream.EndOfStream)
                    {
                        var line = await stream.ReadLineAsync();

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }
                        var fields = line?.Split(',');

                        if (fields != null && fields.Length >= 6)
                        {
                            string dateString = fields[3];
                            DateTime birthDate;
                            bool parseSuccess = DateTime.TryParseExact(
                                dateString,
                                "yyyy-MM-dd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out birthDate
                            );

                            var student = new Student(fields[1], fields[2], birthDate, fields[4], int.Parse(fields[5]));

                            await studentService.UpdateStudentAsync(student);
                        }
                    }
                }
            }

            return RedirectToAction(ViewNames.Index);
        }

    }
}
