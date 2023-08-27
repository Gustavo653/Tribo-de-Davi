namespace TriboDavi.Domain;

public class RollCall : BaseEntity
{
    public FieldOperationTeacher FieldOperationTeacher { get; set; }
    public Student Student { get; set; }
    public DateTime Date { get; set; }
    public bool Presence { get; set; }
}