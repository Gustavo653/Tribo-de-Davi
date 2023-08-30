namespace TriboDavi.Domain;

public class Address : BaseEntity
{
    public required string StreetName { get; set; }
    public required string StreetNumber { get; set; }
    public required string Neighborhood { get; set; }
    public required string City { get; set; }
}