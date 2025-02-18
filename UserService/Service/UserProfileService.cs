using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTO;
using UserService.Model;

namespace UserService.Service
{
    public class UserProfileService
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public UserProfileService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async ValueTask<string> UpdateProfile(string identityUserId, UpdateProfileDTO updateProfileDTO)
        {
            var user = await _context.Users.Where(x => x.IdentityUserId == identityUserId).FirstOrDefaultAsync();

            user = _mapper.Map<User>(updateProfileDTO);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return "User Update Successfully";

        }

        public async ValueTask<string> AddUserAddresses(string identityUserId ,List<UserAddressDto> userAddressDto)
        {
            var user = await GetUserIdByIdentity(identityUserId);
            var userAddresses = _mapper.Map<List<UserAddress>>(userAddressDto);
            userAddresses.ForEach(x => x.UserId = user.Id.ToString());
            _context.AddRange(userAddresses);
            await _context.SaveChangesAsync();
            return "User Addresses Update Successfully";

        }

        private async ValueTask<User> GetUserIdByIdentity(string identityUserId)
        {
            var user =await _context.Users.Where(x => x.IdentityUserId == identityUserId).FirstOrDefaultAsync();
            return user;
        }

        public async ValueTask<UserGetDTO> UserIdByIdentity(string identityUserId)
        {
            var user = await GetUserIdByIdentity(identityUserId);
            var data = _mapper.Map<UserGetDTO>(user);
            return data;
        }

    }
}
