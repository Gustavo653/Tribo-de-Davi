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
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public int? AddressId { get; set; }
        public int? LegalParentId { get; set; }
        public string? EmergencyNumber { get; set; }
    }
}
