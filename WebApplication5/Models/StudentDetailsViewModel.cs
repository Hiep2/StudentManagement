namespace WebApplication5.Models
{
    public class StudentDetailsViewModel
    {
        public required Student Student { get; set; }
        public List<CourseEnrollment> Courses { get; set; } = new List<CourseEnrollment>();
    }
}
