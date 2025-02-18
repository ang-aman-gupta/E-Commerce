namespace UserService.DTO
{
    public class UserGetDTO :UserBaseDTO
    {
        public List<UserAddressDto> Addresses { get; set; }
    }
}
