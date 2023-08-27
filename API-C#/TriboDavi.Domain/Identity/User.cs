using Microsoft.AspNetCore.Identity;

namespace TriboDavi.Domain.Identity
{
    public abstract class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}