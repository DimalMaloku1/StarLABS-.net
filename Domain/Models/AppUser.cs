using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
        public string UserLastname { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }

}