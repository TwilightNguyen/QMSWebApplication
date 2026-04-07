using Microsoft.AspNetCore.Identity.UI.Services;

namespace QMSWebApplication.BackendServer.Services
{
    public class EmailSenderService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here.
            return Task.CompletedTask;
        }
    }
}
