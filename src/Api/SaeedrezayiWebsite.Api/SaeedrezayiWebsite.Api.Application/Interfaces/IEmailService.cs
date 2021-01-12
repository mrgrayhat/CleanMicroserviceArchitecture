using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SaeedrezayiWebsite.Api.Application.DTOs.Email;

namespace SaeedrezayiWebsite.Api.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
