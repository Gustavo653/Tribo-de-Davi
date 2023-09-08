using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Student : User
    {
        public required DateOnly BirthDate { get; set; }
        public required decimal Weight { get; set; }
        public required decimal Height { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public virtual Address? Address { get; set; }
        public virtual required LegalParent LegalParent { get; set; }
        public int CalculateAge()
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - BirthDate.Year;

            if (currentDate.Month < BirthDate.Month || (currentDate.Month == BirthDate.Month && currentDate.Day < BirthDate.Day))
                age--;

            return age;
        }
    }
}
