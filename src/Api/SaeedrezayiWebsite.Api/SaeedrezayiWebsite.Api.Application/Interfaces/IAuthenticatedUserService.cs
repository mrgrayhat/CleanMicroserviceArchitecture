using System;
using System.Collections.Generic;
using System.Text;

namespace SaeedrezayiWebsite.Api.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
    }
}
