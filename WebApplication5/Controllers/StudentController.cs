using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        public StudentController(IStudentService studentRepository)
        {
            this.studentService = studentRepository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            List<Student> students;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                // Warten auf das Ergebnis der Task und dann Konvertieren zu List
                students = (await studentService.GetAllStudentsAsync()).ToList();
            }
            else
            {
                // Warten auf das Ergebnis der Task und dann Konvertieren zu List
                students = (await studentService.SearchStudentsAsync(searchString)).ToList();
            }

            return View(new StudentViewModel()
            {
                Students = students
            });
        }

        public ActionResult Create()
        {
            var student = new Student() { Name = String.Empty};
            return View(ViewNames.CreateEditPartial, student);
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await studentService.GetStudentByIdAsync(id); // Methode, um den Studenten anhand der ID zu finden;
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
                Courses = courseEnrollment
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
                            isFirstLine = false; // Setzen Sie die Flag auf false nach der ersten Zeile
                            continue; // Überspringen der aktuellen Iteration, um die Kopfzeile zu ignorieren
                        }
                        var fields = line?.Split(',');

                        if (fields != null && fields.Length >= 6)
                        {
                            string dateString = fields[3];
                            DateTime birthDate;
                            bool parseSuccess = DateTime.TryParseExact(
                                dateString,
                                "yyyy-MM-dd", // Das erwartete Format
                                CultureInfo.InvariantCulture, // Verwenden Sie CultureInfo.InvariantCulture für ISO 8601-Daten
                                DateTimeStyles.None,
                                out birthDate // Das resultierende DateTime-Objekt, wenn das Parsen erfolgreich war
                            );

                            var student = new Student
                            {
                                Name = fields[1],
                                Sex = fields[2],
                                BirthDate = birthDate,
                                ClassName = fields[4],
                                StudyYear = int.Parse(fields[5])


                            };

                            await studentService.UpdateStudentAsync(student);
                        }
                    }
                }
            }

            return RedirectToAction(ViewNames.Index);
        }

    }
}
