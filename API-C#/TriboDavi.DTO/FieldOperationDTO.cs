using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class FieldOperationDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required AddressDTO Address { get; set; }
    }
}
