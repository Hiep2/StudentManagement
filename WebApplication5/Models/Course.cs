namespace WebApplication5.Models
{
    public class Course
    {
        public int CourseId { get; private set; }
        public string CourseName { get; private set; }
        public int CourseUnit { get; private set; }

        public Course(string courseName, int courseUnit)
        {
            CourseName = courseName;
            CourseUnit = courseUnit;
        }
    }
}
