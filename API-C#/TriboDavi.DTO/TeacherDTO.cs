using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class TeacherDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required int GraduationId { get; set; }
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public string? Password { get; set; }
        [Required]
        public required string RG { get; set; }
        [Required]
        public required string CPF { get; set; }
        public int? MainTeacherId { get; set; }
    }
}
