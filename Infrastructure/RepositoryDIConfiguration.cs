using Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class RepositoryDIConfiguration
    {

        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ILoggingRepository, LoggingRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IDailyTaskRepository, DailyTaskRepository>();


        }

    }
}
