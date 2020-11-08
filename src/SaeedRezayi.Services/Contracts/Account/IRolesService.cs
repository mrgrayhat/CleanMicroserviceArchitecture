using System.Collections.Generic;
using System.Threading.Tasks;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.Services.Contracts.Account
{
    public interface IRolesService
    {
        Task<List<RoleInfo>> FindUserRolesAsync(int userId);
        Task<bool> IsUserInRoleAsync(int userId, string roleName);
        Task<List<UserInfo>> FindUsersInRoleAsync(string roleName);
    }
}
