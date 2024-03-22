namespace WebApplication5.Models
{
    public class CourseEnrollment
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public int CourseUnit { get; set; }

        public double ProzessGrade { get; set; }
        public double ComponentGrade { get; set; }

        public double finalGrade => (ProzessGrade + ComponentGrade) / 2.0;
        public bool isPassed => finalGrade >= 4.0;

        public CourseEnrollment()
        {
            CourseId = 0;
            StudentId = 0;
            CourseName = String.Empty;
            CourseUnit = 0;
            ProzessGrade = 0;
            ComponentGrade = 0;
        }

        public CourseEnrollment(int courseId, string courseName, int courseUnit, double prozessGrade, double componentGrade, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
            CourseName = courseName;
            CourseUnit = courseUnit;
            ProzessGrade = prozessGrade;
            ComponentGrade = componentGrade;
        }

        public void SetStudentId(int studentId)
        {
            StudentId = studentId;
        }
    }
}
