using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class GraduationDTO
    {
        [Required]
        public required string Name { get; set; }
        public required IFormFile File { get; set; }
        [Required]
        public required int Position { get; set; }
    }
}
