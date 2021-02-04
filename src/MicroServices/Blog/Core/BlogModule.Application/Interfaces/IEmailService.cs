using System.Threading.Tasks;
using BlogModule.Application.DTOs.Email;

namespace BlogModule.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
