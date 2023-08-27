namespace TriboDavi.Domain;

public class RollCall : BaseEntity
{
    public required virtual FieldOperationTeacher FieldOperationTeacher { get; set; }
    public required virtual Student Student { get; set; }
    public required virtual DateTime Date { get; set; }
    public required virtual bool Presence { get; set; }
}