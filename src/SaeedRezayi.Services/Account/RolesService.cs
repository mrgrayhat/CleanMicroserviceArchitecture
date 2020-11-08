using System.Linq;
using System.Collections.Generic;
using SaeedRezayi.Common;
using SaeedRezayi.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.Services.Contracts.Account;

namespace SaeedRezayi.Services.Account
{
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RoleInfo> _roles;
        private readonly DbSet<UserInfo> _users;

        public RolesService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _roles = _uow.Set<RoleInfo>();
            _users = _uow.Set<UserInfo>();
        }

        public Task<List<RoleInfo>> FindUserRolesAsync(int userId)
        {
            var userRolesQuery = from role in _roles
                                 from userRoles in role.UserRoles
                                 where userRoles.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            var userRolesQuery = from role in _roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId == userId
                                 select role;
            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public Task<List<UserInfo>> FindUsersInRoleAsync(string roleName)
        {
            var roleUserIdsQuery = from role in _roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;
            return _users.Where(user => roleUserIdsQuery.Contains(user.Id))
                         .ToListAsync();
        }
    }
}