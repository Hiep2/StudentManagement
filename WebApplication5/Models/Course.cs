namespace WebApplication5.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CourseUnit { get; set; }

        public Course(string courseName, int courseUnit)
        {
            CourseName = courseName;
            CourseUnit = courseUnit;
        }
    }
}
