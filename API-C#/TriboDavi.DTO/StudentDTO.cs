using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriboDavi.Domain;

namespace TriboDavi.DTO
{
    public class StudentDTO
    {
        [Required]
        public required DateTime BirthDate { get; set; }
        [Required]
        public required int Weight { get; set; }
        [Required]
        public required int Height { get; set; }
        [Required]
        public required string Graduation { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? SchoolName { get; set; }
        public int? SchoolGrade { get; set; }
        public AddressDTO? Address { get; set; }
        [Required]
        public required LegalParentDTO LegalParent { get; set; }
    }
}
