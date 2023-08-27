using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class BasicDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}