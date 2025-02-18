using Microsoft.EntityFrameworkCore;
using UserService.Model;

namespace UserService.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
        }
    }
}
