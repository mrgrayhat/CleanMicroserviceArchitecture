using System;

namespace BlogModule.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }

        bool IsAuthenticated { get; }
    }
}
