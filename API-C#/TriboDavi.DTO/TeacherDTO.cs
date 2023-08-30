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
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}-\d{1}$", ErrorMessage = "O campo RG não é válido.")]
        public string? RG { get; set; }
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo CPF não é válido.")]
        public string? CPF { get; set; }
        public int? TeacherId { get; set; }
    }
}
