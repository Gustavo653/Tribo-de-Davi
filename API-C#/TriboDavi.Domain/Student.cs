using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Student : User
    {
        public required DateTime BirthDate { get; set; }
        public required decimal Weight { get; set; }
        public required string Url { get; set; }
        public required decimal Height { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public string? EmergencyNumber { get; set; }
        public virtual Address? Address { get; set; }
        public virtual LegalParent? LegalParent { get; set; }
        public virtual IEnumerable<FieldOperationStudent>? FieldOperationStudents { get; set; }
    }
}
