using Org.BouncyCastle.Utilities.Encoders;

namespace WebApplication5.Models
{
    public class Student
    {
        public virtual int StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string? Sex { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string? ClassName { get; set; }
        public virtual int StudyYear { get; set; }


        public Student(string Name, string Sex, DateTime birthDate, string ClassName, int StudyYear)
        {
            this.Name = Name;
            this.Sex = Sex;
            BirthDate = birthDate;
            this.ClassName = ClassName;
            this.StudyYear = StudyYear;
        }

        public Student()
        {
        }
    }
}
