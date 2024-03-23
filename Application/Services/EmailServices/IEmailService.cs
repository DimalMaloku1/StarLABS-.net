using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string message);
        Task SendBookingConfirmationEmailAsync(string email, string message);
        Task SendDailyTaskEmailAsync(string email, string message);

    }
}
