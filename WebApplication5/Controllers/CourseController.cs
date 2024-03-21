using Microsoft.AspNetCore.Mvc;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class CourseController : Controller
    {
        private readonly IStudentService courseEnrollmentRepository;

        public CourseController(IStudentService courseRepository)
        {
            this.courseEnrollmentRepository = courseRepository;
        }

        public async Task<ActionResult> EditAsync(int studentId, int courseId)
        {
            var courseEnrollment = await courseEnrollmentRepository.GetCourseEnrollmentByStudentAsync(studentId, courseId);

            if (courseEnrollment == null)
            {
                return NotFound();
            }

            return View(courseEnrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CourseEnrollment courseEnrollment)
        {
            await courseEnrollmentRepository.UpdateCourseEnrollmentAsync(courseEnrollment);
            return RedirectToAction("Details", "Student", new { id = courseEnrollment.StudentId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int studentId, int courseId)
        {
            await courseEnrollmentRepository.DeleteCourseEnrollmentAsync(courseId, studentId);
            return RedirectToAction("Details", "Student", new { id = studentId });
        }

        public ActionResult Create(int studentId)
        {
            var model = new CourseEnrollment();
            model.SetStudentId(studentId);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(int studentId, CourseEnrollment model)
        {
            if (ModelState.IsValid)
            {
                await courseEnrollmentRepository.AddCourseEnrollmentAsync(studentId, model);

                return RedirectToAction("Details", "Student", new { id = studentId });
            }

            return View(model);
        }

        public async Task<ActionResult> AddAsync()
        {
            var courses = await courseEnrollmentRepository.GetAllCoursesAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<ActionResult> AssignCourseAsync(CourseEnrollment courseEnrollment)
        {
            await courseEnrollmentRepository.AssignCourseEnrollmentToStudentAsync(courseEnrollment);
            return RedirectToAction("Index");
        }
    }
}
