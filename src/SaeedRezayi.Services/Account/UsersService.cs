using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SaeedRezayi.Common;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.DomainClasses.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.ViewModels.Account.Login;
using System.Linq;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.ViewModels.Account.ChangePassword;
using System.Collections.Generic;
using SaeedRezayi.ViewModels.Types;

namespace SaeedRezayi.Services.Account
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserInfo> _users;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UsersService(
            IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _users = _uow.Set<UserInfo>();

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _contextAccessor = contextAccessor;
            _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));
        }
        public async ValueTask<UserInfo> FindUserAsync(int userId)
        {
            return await _users.FindAsync(userId);
        }
        public async Task<UserViewModel> FindUserAsync(int userId, bool include = true)
        {
            UserViewModel user = await _users
                .Include(c => c.Culture)
                .Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                .Include(ur => ur.UserFriends).ThenInclude(x => x.User2)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == userId);
            return user;
            //return await _users.FindAsync(userId);
        }

        public async Task<UserInfo> FindUserAsync(LoginRequestViewModel loginRequest)
        {
            var passwordHash = _securityService.GetSha256Hash(loginRequest.Password);
            var query = _users.AsNoTracking();
            // if email has'nt value, search for username
            if (string.IsNullOrWhiteSpace(loginRequest.Email))
            {
                query = query
                    .Where(x => x.Username == loginRequest.Username && x.Password == passwordHash);
            }
            else // email has value so search for email
            {
                query = query
                    .Where(x => x.EmailAddress == loginRequest.Email && x.Password == passwordHash);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<string> GetSerialNumberAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            return user.SerialNumber;
        }

        public async Task UpdateUserLastActivityDateAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            if (user.LastLoggedIn != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }
            user.LastLoggedIn = DateTimeOffset.UtcNow;
            await _uow.SaveChangesAsync();
        }

        public int GetCurrentUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            var userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
        }

        /// <summary>
        /// get all user's information (Paging)
        /// </summary>
        /// <param name="sortField">sort based on this field, default is Id</param>
        /// <param name="maxRecords">max record to fetch</param>
        /// <param name="sortingOrder">Sort Type, Default is descending</param>
        /// <returns>paged list of users</returns>
        public async Task<IEnumerable<UserViewModel>> GetPagedUsersListAsync(
            string sortField = "Id",
            int maxRecords = 10,
            SortingOrderTypes sortingOrder = SortingOrderTypes.Descending)
        {
            return await _users
                .Include(x => x.UserFriends)
                .Select(x => (UserViewModel)x)
                .AsNoTracking()
                .ToListAsync();
        }

        public async ValueTask<UserInfo> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            return await FindUserAsync(userId);
        }

        public async Task<ChangePasswordResponseViewModel> ChangePasswordAsync(UserInfo user, ChangePasswordRequestViewModel requestViewModel)
        {
            if (requestViewModel.NewPassword.Length < 8)
            {
                return new ChangePasswordResponseViewModel
                {
                    Succeeded = false,
                    Error = "Password must be greater than 8 character.",
                    StatusCode = Common.Messages.MessageStatusCodeTypes.INVALID
                };
            }
            var currentPasswordHash = _securityService.GetSha256Hash(requestViewModel.OldPassword);
            if (user.Password != currentPasswordHash)
            {
                return new ChangePasswordResponseViewModel
                {
                    Succeeded = false,
                    Error = "Current password is wrong.",
                    StatusCode = Common.Messages.MessageStatusCodeTypes.INVALID
                };
            }
            if (string.CompareOrdinal(requestViewModel.NewPassword,
                requestViewModel.ConfirmPassword) != 0)
            {
                return new ChangePasswordResponseViewModel
                {
                    Succeeded = false,
                    Error = "Password not match with confirm password! ",
                    StatusCode = Common.Messages.MessageStatusCodeTypes.INVALID
                };
            }
            user.Password = _securityService.GetSha256Hash(requestViewModel.NewPassword);
            // user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
            await _uow.SaveChangesAsync();
            return new ChangePasswordResponseViewModel
            {
                Succeeded = true,
                StatusCode = Common.Messages.MessageStatusCodeTypes.SUCCESS
            };
        }
    }
}
