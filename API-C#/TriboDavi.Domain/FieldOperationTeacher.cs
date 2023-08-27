namespace TriboDavi.Domain;

public class FieldOperationTeacher : BaseEntity
{
    public required Teacher Teacher { get; set; }
    public Teacher AssistantTeacher { get; set; }
    public required FieldOperation FieldOperation { get; set; }
    public bool Enabled { get; set; }
}