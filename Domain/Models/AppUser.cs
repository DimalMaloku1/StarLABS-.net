using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Domain.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        [PersonalData]
        [Column(TypeName ="nvarchar(100)")]
        public string firstName { get; set; }

		[PersonalData]
		[Column(TypeName = "nvarchar(100)")]
		public string lastName { get; set; }

        public AppUser()
        {
            Role = "Guest";
        }

        public string? Role { get; set; }
    }

    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(AppUser user)
        {
            var result = await base.CreateAsync(user);

            if (result.Succeeded)
            {
                await AddToRoleAsync(user, "Guest"); // Set default role as Guest when a new user is created
            }

            return result;
        }
    }
}
