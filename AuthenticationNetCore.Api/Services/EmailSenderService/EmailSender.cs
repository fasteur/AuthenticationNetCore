using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AuthenticationNetCore.Api.Services.EmailSenderService
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options, subject, message, email);
        }

        static async Task Execute(AuthMessageSenderOptions options, string subject, string message, string email)
        {
            var client = new SendGridClient(options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("asteur.florian@gmail.com", options.SendGridUser),
                ReplyTo = new EmailAddress("asteur.florian@gmail.com", options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            // Disable click tracking. 
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }

    }
}