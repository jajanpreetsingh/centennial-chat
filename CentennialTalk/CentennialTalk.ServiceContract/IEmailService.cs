using SendGrid;
using System.Threading.Tasks;

namespace CentennialTalk.ServiceContract
{
    public interface IEmailService
    {
        Task<Response> SendEmail(string apiKey, string subject, string message, string email);
    }
}