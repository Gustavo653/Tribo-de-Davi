using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Teacher : User
    {
        public required string Graduation { get; set; } // Analisar
        public required string RG { get; set; }
        public required string CPF { get; set; }
        public virtual Teacher AssistantTeacher { get; set; }
    }
}
