using AuthService.DBContext;
using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AuthService.Services
{
    public class RegisterUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthServiceDbContext _authServiceDbContext;

        public RegisterUserService(UserManager<AppUser> userManager, AuthServiceDbContext authServiceDbContext)
        {
            _userManager = userManager;
            _authServiceDbContext = authServiceDbContext;
        }

        public async ValueTask<string> RegisterCustomerUserAsync(RegisterUserDTO registerUserDTO)
        {
            var user = new AppUser
            {
                UserName = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                
            };
           var userResult = await _userManager.CreateAsync(user,registerUserDTO.Password);
            if (userResult.Succeeded)
            {
                var roleAddedResult = await _userManager.AddToRoleAsync(user, "Customer");
                if (roleAddedResult.Succeeded) 
                {
                    var outboxEvent = new OutBoxEvent
                    {
                        EventType = "UserCreated",
                        Payload = JsonSerializer.Serialize(
                                    new 
                                    { 
                                      user.Id,
                                      registerUserDTO.FirstName,
                                      registerUserDTO.LastName,
                                      registerUserDTO.Email,
                                      registerUserDTO.Phone,
                                    })
                        
                    };
                    await _authServiceDbContext.OutBoxEvents.AddAsync(outboxEvent);
                    await _authServiceDbContext.SaveChangesAsync();
                    return "User Created Successfully";

                }
                return "User Created but Added in Role";
            }
            else
            {
                return "User Not Created.";
            }
            
        }

        public async ValueTask<string> RegisterSellerUserAsync(RegisterUserDTO registerUserDTO)
        {
            var user = new AppUser
            {
                UserName = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                
            };
            var userResult = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (userResult.Succeeded)
            {
                var roleAddedResult = await _userManager.AddToRoleAsync(user, "Seller");
                if (roleAddedResult.Succeeded)
                {
                    var outboxEvent = new OutBoxEvent
                    {
                        EventType = "UserCreated",
                        Payload = JsonSerializer.Serialize(new { user.Id, registerUserDTO })

                    };
                    await _authServiceDbContext.OutBoxEvents.AddAsync(outboxEvent);
                    await _authServiceDbContext.SaveChangesAsync();
                    return "User Created Successfully";

                }
                return "User Created but Added in Role";
            }
            else
            {
                return "User Not Created.";
            }
        }

    }
}
