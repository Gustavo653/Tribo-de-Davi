namespace TriboDavi.Domain;

public class FieldOperationTeacher : BaseEntity
{
    public required virtual Teacher Teacher { get; set; }
    public required virtual FieldOperation FieldOperation { get; set; }
    public required bool Enabled { get; set; }
}