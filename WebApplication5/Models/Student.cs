namespace WebApplication5.Models
{
    public class Student
    {
        public virtual int StudentId { get; set; }
        public required virtual string Name { get; set; }
        public virtual string? Sex { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string? ClassName { get; set; }
        public virtual int StudyYear { get; set; }
    }
}
