using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class RoleService
    {
        public static async Task SeedIniData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<AppRole>()
                {
                    new(){Name = "Admin"},
                    new(){Name = "Seller"},
                    new(){Name = "Customer"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

            }
            if (!userManager.Users.Any())
            {
                var users = new AppUser
                {
                    UserName = "SystemAdmin",
                    Email = "systemAdmin@test.com"
                     
                    
                };
                await userManager.CreateAsync(users, "Stagep@321");
                await userManager.AddToRoleAsync(users,"Admin");

            }
        }
    }
}
