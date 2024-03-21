using Microsoft.AspNetCore.Mvc;
using WebApplication5.Models;
using WebApplication5.Services;

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
            var courseEnrollment = await studentService.GetCourseEnrollmentByStudentAsync(studentId, courseId);

            if (courseEnrollment == null)
            {
                return NotFound(); // Vergewissern Sie sich, dass NotFound korrekt gehandhabt wird
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
                    await studentService.AddCourseEnrollmentAsync(studentId, courseEnrollment);
                }
                else
                {
                    await studentService.UpdateCourseEnrollmentAsync(courseEnrollment);
                }

                return RedirectToAction(ViewNames.Details, ViewNames.Student, new { id = courseEnrollment.StudentId });
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
            await studentService.AssignCourseToStudentAsync(courseEnrollment);
            return RedirectToAction(ViewNames.Index);
        }
    }
}
