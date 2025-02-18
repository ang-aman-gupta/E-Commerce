using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.ModelMetadata
{
    public static class IdentityMetadata
    {
        public static void ModelConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(e =>
                e.ToTable(name: "Users", schema: "Auth"));
            modelBuilder.Entity<AppRole>(e =>
                e.ToTable(name: "Roles", schema: "Auth"));
            modelBuilder.Entity<IdentityRoleClaim<string>>(e =>
                e.ToTable(name: "RoleClaims", schema: "Auth"));
            modelBuilder.Entity<IdentityUserClaim<string>>(e =>
                e.ToTable(name: "UserClaims", schema: "Auth"));
            modelBuilder.Entity<IdentityUserLogin<string>>(e =>
                e.ToTable(name: "UserLogins", schema: "Auth")); 
            modelBuilder.Entity<IdentityUserToken<string>>(e =>
                e.ToTable(name: "UserTokens", schema: "Auth"));
            modelBuilder.Entity<IdentityUserRole<string>>(e =>
                e.ToTable(name: "UserRoles", schema: "Auth"));

            

        }
    }
}
