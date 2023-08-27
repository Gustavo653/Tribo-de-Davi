using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class AmbulanceDTO
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int IdChecklist { get; set; }
    }
}
