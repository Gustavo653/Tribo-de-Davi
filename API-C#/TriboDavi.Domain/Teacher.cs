using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Teacher : User
    {
        public virtual Teacher? MainTeacher { get; set; }
    }
}
