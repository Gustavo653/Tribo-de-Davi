using Microsoft.AspNetCore.Identity;

namespace TriboDavi.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public required override string Email { get; set; }
        public required string Name { get; set; }
        public required Graduation Graduation { get; set; }
        public required string RG { get; set; }
        public required string CPF { get; set; }
        public virtual IEnumerable<UserRole>? UserRoles { get; set; }
    }
}