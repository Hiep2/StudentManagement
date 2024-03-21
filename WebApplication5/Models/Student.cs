using Org.BouncyCastle.Utilities.Encoders;

namespace WebApplication5.Models
{
    public class Student
    {
        public virtual int StudentId { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Sex { get; private set; }
        public virtual DateTime BirthDate { get; private set; }
        public virtual string ClassName { get; private set; }
        public virtual int StudyYear { get; private set; }

        public Student(string name, string sex, DateTime birthDate, string className, int studyYear)
        {
            Name = name;
            Sex = sex;
            BirthDate = birthDate;
            ClassName = className;
            StudyYear = studyYear;
        }

        public Student()
        {
            Name = String.Empty;
            Sex = String.Empty;
            BirthDate = new DateTime();
            ClassName = String.Empty;
            StudyYear = 0;
        }
    }
}
