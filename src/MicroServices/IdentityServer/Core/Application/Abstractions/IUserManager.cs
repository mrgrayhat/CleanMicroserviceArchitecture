using System;
using System.Threading.Tasks;
using STS.Application.Common.Models;

namespace STS.Application.Abstractions
{
    public interface IUserManager
    {
        Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(Guid userId);
    }
}
