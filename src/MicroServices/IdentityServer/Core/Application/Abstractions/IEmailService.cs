using System.Threading.Tasks;
using STS.Application.Features.Notifications.Models;

namespace STS.Application.Abstractions
{
    public interface IEmailService
    {
        Task RegistrationConfirmationEmail(string to, string link);
        Task ForgottentPasswordEmail(string to, string link);
        Task SendCustomerCreatedEmail(EmailMessage emailMessage);
    }
}
