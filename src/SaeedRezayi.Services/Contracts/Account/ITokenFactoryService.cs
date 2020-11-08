using System.Threading.Tasks;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.ViewModels.Account;

namespace SaeedRezayi.Services.Contracts.Account
{
    public interface ITokenFactoryService
    {
        Task<JwtTokensDataViewModel> CreateJwtTokensAsync(UserViewModel user);
        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
