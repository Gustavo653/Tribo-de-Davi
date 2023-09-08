using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
