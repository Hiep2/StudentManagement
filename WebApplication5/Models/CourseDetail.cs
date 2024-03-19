namespace WebApplication5.Models
{
    public class CourseDetail
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public int CourseUnit { get; set; }

        public double ProzessGrade { get; set; }
        public double ComponentGrade { get; set; }

        public double finalGrade => (ProzessGrade + ComponentGrade) / 2.0;
        public bool isPassed => finalGrade >= 4.0;
    }
}
