using AuthService.ModelMetadata;
using AuthService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DBContext
{
    public class AuthServiceDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public DbSet<OutBoxEvent> OutBoxEvents { get; set; }
        public AuthServiceDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            IdentityMetadata.ModelConfiguration(builder);
            
        }
    }
}
