namespace TriboDavi.Domain;

public class RollCall : BaseEntity
{
    public required virtual FieldOperationStudent FieldOperationStudent { get; set; }
    public required DateOnly Date { get; set; }
    public required bool Presence { get; set; }
}