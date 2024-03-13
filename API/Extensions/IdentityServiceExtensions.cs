using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Infrastructure;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen();
            services.AddIdentity<AppUser, IdentityRole<Guid>>().AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
                ;
            services.Configure<IdentityOptions>(options =>
            {

                options.Password.RequireDigit = false; // Do not require a digit (number).
                options.Password.RequireLowercase = false; // Do not require a lowercase letter.
                options.Password.RequireUppercase = false; // Do not require an uppercase letter.
                options.Password.RequireNonAlphanumeric = false; // Do not require a special character.
                options.Password.RequiredLength = 1; // Set the minimum password length you desire.
                options.SignIn.RequireConfirmedEmail = true;
            });
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            return services;
        }
    }
}
