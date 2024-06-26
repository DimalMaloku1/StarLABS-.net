using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.FeedbackServices;
using Application.Services.PaymentServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.EmailServices;
using Application.Services.StaffServices;
using Application.Stripe;
using Application.Validations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.TokenService;
using Application.Services.AccountServices;
using Application.DTOs.AccountDTOs;
using Application.Services.PositionServices;
using Application.Services.DashboardService;
using Application.Services.LoggingServices;
using Application.Services.RazorServices;
using Application.Services.DailyTaskServices;
using Application.PayPal;
using Application.Services.PaymentServices.Stripe;
using Application.Services.PaymentServices.PaymentSuccess;
using Application.Services.PaymentServices.UnitWork;
using Application.Services.PaymentServices.PaymentInitializer;


namespace Application
{
    public class ServicesDIConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IPositionServices, PositionServices>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IValidator<BookingDto>, BookingValidator>();
            services.AddTransient<IValidator<RoomTypeDto>, RoomTypeValidator>();
            services.AddTransient<IValidator<RoomDto>, RoomValidator>();
            services.AddTransient<IValidator<StaffDTO>, StaffValidator>();
            services.AddTransient<IValidator<PaymentDto>, PaymentValidator>();
            services.AddTransient<IValidator<BillDto>, BillValidator>();
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            services.Configure<PayPalSettings>(configuration.GetSection("PayPal"));
            services.AddScoped<IPaymentInitializer, PaymentInitializer>();
            services.AddScoped<IPaymentSuccess, PaymentSuccess>();
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoomServices, RoomServices>();
            services.AddScoped<IRoomTypeServices, RoomTypeServices>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IValidator<LoginDto>, LoginValidator>();
            services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IDailyTaskService, DailyTaskService>();
            services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();
        }
    }
}