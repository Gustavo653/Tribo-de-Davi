using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class PresenceDTO
    {
        [Required]
        public required int StudentId { get; set; }
        [Required]
        public required DateOnly Date { get; set; }
        [Required]
        public required bool Presence { get; set; }
    }
}
