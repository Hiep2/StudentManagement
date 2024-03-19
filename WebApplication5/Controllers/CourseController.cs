using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class CourseController : Controller
    {
        private readonly IStudentService courseRepository;

        public CourseController(IStudentService courseRepository)
        {
            this.courseRepository = courseRepository ;
        }

        public async Task<ActionResult> EditAsync(int studentId, int courseId)
        {
            var courseDetail = await courseRepository.GetCourseDetailByStudentAsync(studentId, courseId);

            if (courseDetail == null)
            {
                return NotFound(); // Vergewissern Sie sich, dass NotFound korrekt gehandhabt wird
            }

            return View(courseDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CourseDetail courseDetail)
        {
            await courseRepository.UpdateCourseEnrollmentAsync(courseDetail);
            return RedirectToAction("Details", "Student", new { id = courseDetail.StudentId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int studentId, int courseId)
        {
            await courseRepository.DeleteCourseEnrollmentAsync(courseId, studentId);
            return RedirectToAction("Details", "Student", new { id = studentId });
        }

        public ActionResult Create(int studentId)
        {
            var model = new CourseDetail
            {
                StudentId = studentId // Stellen Sie sicher, dass dieser Wert korrekt gesetzt ist.
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(int studentId, CourseDetail model)
        {
            if (ModelState.IsValid)
            {
                await courseRepository.AddCourseDetailAsync(studentId, model);

                return RedirectToAction("Details", "Student", new { id = studentId });
            }

            return View(model);
        }

        public async Task<ActionResult> AddAsync()
        {
            var courses = await courseRepository.GetAllCoursesAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<ActionResult> AssignCourseAsync(CourseDetail courseDetail)
        {
            await courseRepository.AssignCourseToStudentAsync(courseDetail);
            return RedirectToAction("Index");
        }
    }
}
