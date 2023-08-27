namespace TriboDavi.Domain;

public class FieldOperation : BaseEntity
{
    public required string Name { get; set; }
    public required virtual Address Address { get; set; }
}