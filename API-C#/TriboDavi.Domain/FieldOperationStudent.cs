namespace TriboDavi.Domain
{
    public class FieldOperationStudent : BaseEntity
    {
        public required virtual FieldOperationTeacher FieldOperationTeacher { get; set; }
        public required virtual Student Student { get; set; }
        public bool Enabled { get; set; }
    }
}