﻿using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class ChecklistItemDTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int RequiredQuantity { get; set; }
        [Required]
        public int IdCategory { get; set; }
        [Required]
        public int IdItem { get; set; }
        [Required]
        public int IdChecklist { get; set; }
    }
}
