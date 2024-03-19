namespace WebApplication5.Models
{
    public class StudentDetailsViewModel
    {
        public Student Student { get; set; }
        public List<CourseDetail> Courses { get; set; } = new List<CourseDetail>();
    }
}
