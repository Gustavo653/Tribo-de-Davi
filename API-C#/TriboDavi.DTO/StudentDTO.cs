using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class StudentDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required DateTime BirthDate { get; set; }
        [Required]
        public required int Weight { get; set; }
        [Required]
        public required int Height { get; set; }
        [Required]
        public required string Graduation { get; set; }
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public string? Password { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public AddressDTO? Address { get; set; }
        public LegalParentDTO? LegalParent { get; set; }
        public int? LegalParentId { get; set; }
    }
}
