using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Student : User
    {
        public required DateTime BirthDate { get; set; }
        public required int Weight { get; set; }
        public required int Height { get; set; }
        public required string Graduation { get; set; } // Analisar
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public virtual Address? Address { get; set; }
        public virtual required LegalParent LegalParent { get; set; }
    }
}
