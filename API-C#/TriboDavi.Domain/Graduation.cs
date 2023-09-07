namespace TriboDavi.Domain
{
    public class Graduation : BaseEntity
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required int Position { get; set; }
    }
}
