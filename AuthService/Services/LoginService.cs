using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class LoginService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        

        public LoginService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async ValueTask<string> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if (user == null)
            {
                return "Invalid Username";
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password,false);
            if (!result.Succeeded)
            {
                return "Invalid Password";
            }
            return await _tokenService.GenerateTokenAsync(user);
        }
    }
}
