using Microsoft.AspNetCore.Mvc;
using WebApplication5.IRepositories;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class CourseController : Controller
    {
        private readonly IStudentService studentService;

        public CourseController(IStudentService courseRepository)
        {
            this.studentService = courseRepository;
        }

        public async Task<ActionResult> EditAsync(int studentId, int courseId)
        {
            var courseEnrollment = await studentService.GetCourseEnrollmentFromStudentAsync(studentId, courseId);

            if (courseEnrollment == null)
            {
                return NotFound();
            }

            return View(ViewNames.CreateEditPartial, courseEnrollment);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int studentId, int courseId)
        {
            await studentService.DeleteCourseEnrollmentAsync(courseId, studentId);
            return RedirectToAction(ViewNames.Details, ViewNames.Student, new { id = studentId });
        }

        public ActionResult Create(int studentId)
        {
            var courseEnrollment = new CourseEnrollment
            {
                CourseName = String.Empty,
                StudentId = studentId
            };
            return View(ViewNames.CreateEditPartial, courseEnrollment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAsync(int studentId, CourseEnrollment courseEnrollment)
        {
            if (ModelState.IsValid)
            {

                if (courseEnrollment.CourseId == 0)
                {
                    await studentService.AddCourseEnrollmentToStudentAsync(studentId, courseEnrollment);
                }
                else
                {
                    await studentService.UpdateCourseEnrollmentAsync(courseEnrollment);
                }

                return RedirectToAction(ViewNames.Details, ViewNames.Student, new { id = courseEnrollment.StudentId });
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;

                        // Loggen Sie den Schlüssel und die Fehlermeldung
                        String test = $"Key: {key}, Error: {errorMessage}";
                        Console.WriteLine($"Key: {key}, Error: {errorMessage}");

                        // Wenn eine Ausnahme vorhanden ist, loggen Sie auch diese
                        if (exception != null)
                        {
                            Console.WriteLine($"Exception: {exception.Message}");
                        }
                    }
                }
            }
                return View(ViewNames.CreateEditPartial, courseEnrollment);
        }

        public async Task<ActionResult> AddAsync()
        {
            var courses = await studentService.GetAllCoursesAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<ActionResult> AssignCourseAsync(CourseEnrollment courseEnrollment)
        {
            await studentService.AssignCourseEnrollmentToStudentAsync(courseEnrollment);
            return RedirectToAction(ViewNames.Index);
        }
    }
}
