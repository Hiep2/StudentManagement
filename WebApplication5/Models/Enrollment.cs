namespace WebApplication5.Models
{
    public class Enrollment
    {
        public virtual int EnrollmentId { get; private set; }
        public virtual int CourseId { get; private set; }
        public virtual Course Course { get; private set; }
        public virtual int StudentId { get; private set; }
        public virtual double ProzessGrade { get; private set; }
        public virtual double ComponentGrade { get; private set; }

        public double finalGrade => (ProzessGrade + ComponentGrade) / 2.0;
        public bool isPassed => finalGrade >= 4.0;

        public Enrollment(int courseId, int studentId, double prozessGrade, double componentGrade)
        {
            CourseId = courseId;
            StudentId = studentId;
            ProzessGrade = prozessGrade;
            ComponentGrade = componentGrade;
        }

        internal void SetComponentGrade(double componentGrade)
        {
            ComponentGrade = componentGrade;
        }

        internal void SetProzessGrade(double prozessGrade)
        {
            ProzessGrade = prozessGrade;
        }
    }
}
