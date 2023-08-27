using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TriboDavi.Domain;
using TriboDavi.Domain.Identity;

namespace TriboDavi.Persistence
{
    public class TriboDaviContext : IdentityDbContext<User, Role, int,
                                               IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                               IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public TriboDaviContext(DbContextOptions<TriboDaviContext> options) : base(options) { }

        protected TriboDaviContext()
        {
        }

        public DbSet<LegalParent> LegalParents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Address>();

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            }
           );
        }
    }
}
