using Application.Core;
using Application;
using Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews(opt =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            var key = config.GetValue<string>("StripeSettings:SecretKey");

            RepositoryDIConfiguration.Configure(services);
            ServicesDIConfiguration.Configure(services,config);

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                opt.LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            return services;
        }
    }
}


