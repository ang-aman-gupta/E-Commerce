using AuthService.DTOs;
using AuthService.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly RegisterUserService _registerUserService;
        public UserRegistrationController(RegisterUserService registerUserService)
        {
                _registerUserService = registerUserService;
        }


        [HttpPost]
        public async ValueTask<IActionResult> RegisterCustomerAsync(RegisterUserDTO registerUserDTO)
        {
            var result = await _registerUserService.RegisterCustomerUserAsync(registerUserDTO);
            return Ok(result);
        }

        [HttpPost]
        public async ValueTask<IActionResult> RegisterSellerAsync(RegisterUserDTO registerUserDTO)
        {
            var result = await _registerUserService.RegisterSellerUserAsync(registerUserDTO);
            return Ok(result);
        }
    }
}
