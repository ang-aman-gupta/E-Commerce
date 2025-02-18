using AuthService.DTOs;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> LoginUserAsync(LoginDTO loginDTO)
        {
            var result = await _loginService.LoginAsync(loginDTO);
            return Ok(result);
        }
    }
}
