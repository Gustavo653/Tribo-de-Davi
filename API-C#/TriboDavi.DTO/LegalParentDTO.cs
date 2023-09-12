using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class LegalParentDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Relationship { get; set; }
        [Required]
        //[RegularExpression(@"^\d{2}\.\d{3}\.\d{3}-\d{1}$", ErrorMessage = "O campo RG não é válido.")]
        public required string RG { get; set; }
        [Required]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo CPF não é válido.")]
        public required string CPF { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
