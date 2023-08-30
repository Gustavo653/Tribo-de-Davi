using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class GraduationDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Url { get; set; }
        [Required]
        public required int Position { get; set; }
    }
}
