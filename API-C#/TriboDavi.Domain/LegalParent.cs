namespace TriboDavi.Domain
{
    public class LegalParent : BaseEntity
    {
        public required string Name { get; set; }
        public required string Relationship { get; set; } // Analisar
        public required string RG { get; set; }
        public required string CPF { get; set; }
        public required string PhoneNumber { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}
