using System;
using System.Security.Claims;
using BlogModule.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BlogModule.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            UserId = userId;
            IsAuthenticated = UserId != null;
        }

        public Guid UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
