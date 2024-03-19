using Dapper.FluentMap.Mapping;

namespace WebApplication5.Models
{
    public class StudentMap : EntityMap<Student>
    {
        public StudentMap()
        {
            Map(x => x.StudentId);
            Map(x => x.Name);
            Map(x => x.Sex);
            Map(x => x.BirthDate);
            Map(x => x.ClassName);
            Map(x => x.StudyYear);
        }
    }
}
