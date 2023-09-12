using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class StudentDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required DateTime BirthDate { get; set; }
        public IFormFile? File { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public required decimal Weight { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public required decimal Height { get; set; }
        [Required]
        public required int GraduationId { get; set; }
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public string? Password { get; set; }
        //[RegularExpression(@"^\d{2}\.\d{3}\.\d{3}-\d{1}$", ErrorMessage = "O campo RG não é válido.")]
        public string? RG { get; set; }
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo CPF não é válido.")]
        public string? CPF { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public AddressDTO? Address { get; set; }
        [Required]
        public required LegalParentDTO LegalParent { get; set; }
    }
}
