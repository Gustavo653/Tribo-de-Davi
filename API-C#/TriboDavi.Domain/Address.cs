namespace TriboDavi.Domain;

public abstract class Address
{
    public string StreetName { get; set; }
    public string StreetNumber { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
}