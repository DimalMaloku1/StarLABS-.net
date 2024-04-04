using Application.Base;

namespace Application.DTOs.AccountDTOs
{
    public class UserDto : BaseEntityDto
    {
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        //public string Token { get; set; }

    }
}
