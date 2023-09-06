using System.ComponentModel.DataAnnotations;

namespace TriboDavi.DTO
{
    public class FieldOperationStudentDTO
    {
        [Required]
        public required int FieldOperationTeacherId { get; set; }
        [Required]
        public required int StudentId { get; set; }
        [Required]
        public required bool Enabled { get; set; }
    }
}
