using System.Collections.Generic;
using System.Threading.Tasks;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.ViewModels.Account.ChangePassword;
using SaeedRezayi.ViewModels.Account.Login;
using SaeedRezayi.ViewModels.Types;

namespace SaeedRezayi.Services.Contracts.Account
{
    public interface IUsersService
    {
        #region Find/Get
        int GetCurrentUserId();
        ValueTask<UserInfo> GetCurrentUserAsync();
        Task<string> GetSerialNumberAsync(int userId);
        Task<UserInfo> FindUserAsync(LoginRequestViewModel loginRequest);
        Task<UserViewModel> FindUserAsync(int userId, bool include = true);
        ValueTask<UserInfo> FindUserAsync(int userId);
        Task<IEnumerable<UserViewModel>> GetPagedUsersListAsync(string sortField = "Id", int maxRecords = 10, SortingOrderTypes sortingOrder = SortingOrderTypes.Descending);
        #endregion

        #region Add/Update
        Task UpdateUserLastActivityDateAsync(int userId);
        Task<ChangePasswordResponseViewModel> ChangePasswordAsync(UserInfo user, ChangePasswordRequestViewModel passwordViewModel);
        #endregion


    }
}
