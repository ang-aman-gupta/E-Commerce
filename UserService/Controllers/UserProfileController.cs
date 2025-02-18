using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.DTO;
using UserService.Service;

namespace UserService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost]
        [Authorize]
        public async ValueTask<IActionResult> AddUserAddressAsync(List<UserAddressDto> userAddressDtos)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _userProfileService.AddUserAddresses(userId,userAddressDtos);
            return Ok(result);

        }

        [HttpPost]
        [Authorize]
        public async ValueTask<IActionResult> AddUserProfileAsync(UpdateProfileDTO updateProfileDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _userProfileService.UpdateProfile(userId, updateProfileDto);
            return Ok(result);

        }

        [HttpGet]
        [Authorize]
        public async ValueTask<IActionResult> GetUserProfileAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _userProfileService.UserIdByIdentity(userId);
            return Ok(result);

        }


    }
}
