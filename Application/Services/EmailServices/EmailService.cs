using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace Application.Services.EmailServices
{
    internal sealed class EmailService : IEmailService
    {
        private UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmailService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendVerificationEmailAsync(string email, string message)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("ffc0ccb988868a", "0eb8938323e50e"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("hotelmanagementsystem138@gmail.com"),
                Subject = "Verify your email",
                IsBodyHtml = true,
                Body = message
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);

        }
        public async Task SendBookingConfirmationEmailAsync(string email, string message)
        {
            SmtpClient client = new SmtpClient("sandbox.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("ffc0ccb988868a", "0eb8938323e50e"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("systemhotelmanagment@gmail.com"),
                Subject = "Booking Confirmation",
                IsBodyHtml = true,
                Body = message

            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendDailyTaskEmailAsync(string email, string message)
        {

            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("ffc0ccb988868a", "0eb8938323e50e"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("systemhotelmanagment@gmail.com"),
                Subject = "New Daily Task Assigned",
                IsBodyHtml = true,
                Body = message
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

    }
}
