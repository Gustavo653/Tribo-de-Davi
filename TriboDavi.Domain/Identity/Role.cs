using Microsoft.AspNetCore.Identity;

namespace TriboDavi.Domain.Identity
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}