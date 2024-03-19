using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using Org.BouncyCastle.Crypto.Signers;
using System.Globalization;
using System.Xml.Linq;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentRepository;
        public StudentController(IStudentService studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            List<Student> students;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                // Warten auf das Ergebnis der Task und dann Konvertieren zu List
                students = (await studentRepository.GetAllStudentsAsync()).ToList();
            }
            else
            {
                // Warten auf das Ergebnis der Task und dann Konvertieren zu List
                students = (await studentRepository.SearchStudentsAsync(searchString)).ToList();
            }

            return View(new StudentViewModel()
            {
                Students = students
            });
        }



        public ActionResult Create()
        {
            var student = new Student();
            return View("CreateEditPartial", student);
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await studentRepository.GetStudentByIdAsync(id); // Methode, um den Studenten anhand der ID zu finden;
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
            await studentRepository.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditAsync(int id)
        {
            var student = await studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View("CreateEditPartial", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAsync(Student studentModel)
        {
            if (ModelState.IsValid)
            {
                if(studentModel.StudentId == 0)
                {
                    await studentRepository.AddStudentAsync(studentModel);
                }
                else
                {
                    await studentRepository.UpdateStudentAsync(studentModel);
                }

                return RedirectToAction("Index");
            }

            return View("CreateOrEdit", studentModel);
        }

        public async Task<ActionResult> Details(int id)
        {
            var student = await studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var courseDetail = await studentRepository.GetCourseDetailsByStudentIdAsync(id);

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                Courses = courseDetail
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

                            studentRepository.UpdateStudentAsync(student);
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ImportXML(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var xmlDoc = XDocument.Load(stream);
                    // Verwenden Sie hier die XML-Dokumentation, um die Daten zu extrahieren und in die Datenbank einzufügen
                }
            }

            return RedirectToAction("Index");
        }



    }
}
