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
        public required string RG { get; set; }
        [Required]
        public required string CPF { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
