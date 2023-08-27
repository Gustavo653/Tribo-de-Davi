using Microsoft.AspNetCore.Identity;

namespace TriboDavi.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public required string Name { get; set; }
        public required string Graduation { get; set; } // Analisar
        public required string RG { get; set; }
        public required string CPF { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}