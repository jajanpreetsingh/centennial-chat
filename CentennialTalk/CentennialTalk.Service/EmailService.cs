using CentennialTalk.ServiceContract;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CentennialTalk.Service
{
    public class EmailService : IEmailService
    {
        public Task<Response> SendEmail(string apiKey, string subject, string message, string email)
        {
            SendGridClient client = new SendGridClient(apiKey);

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress("jsing787@my.centennialcollege.ca", "Voice it - Centennial College"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            // Disable click tracking. See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}