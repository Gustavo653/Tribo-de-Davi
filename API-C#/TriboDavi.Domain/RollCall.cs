namespace TriboDavi.Domain;

public class RollCall : BaseEntity
{
    public required virtual FieldOperationStudent FieldOperationStudent { get; set; }
    public required virtual DateTime Date { get; set; }
    public required virtual bool Presence { get; set; }
}