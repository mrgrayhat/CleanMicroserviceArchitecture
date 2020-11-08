using System;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.ViewModels.Account
{
    public class UserTokenViewModel
    {
        public int Id { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        //public int UserId { get; set; }
        //public virtual User User { get; set; }

        public static implicit operator UserTokenViewModel(UserTokenInfo userToken)
        {
            if (userToken == null)
                return null;

            return new UserTokenViewModel
            {
                Id = userToken.Id,
                AccessTokenExpiresDateTime = userToken.AccessTokenExpiresDateTime,
                AccessTokenHash = userToken.AccessTokenHash,
                RefreshTokenExpiresDateTime = userToken.RefreshTokenExpiresDateTime,
                RefreshTokenIdHash = userToken.RefreshTokenIdHash,
                RefreshTokenIdHashSource = userToken.RefreshTokenIdHashSource
            };
        }

        public static implicit operator UserTokenInfo(UserTokenViewModel userTokenViewModel)
        {
            if (userTokenViewModel == null)
                return null;

            return new UserTokenInfo
            {
                Id = userTokenViewModel.Id,
                RefreshTokenIdHashSource=userTokenViewModel.RefreshTokenIdHashSource,
                RefreshTokenIdHash = userTokenViewModel.RefreshTokenIdHash,
                RefreshTokenExpiresDateTime = userTokenViewModel.RefreshTokenExpiresDateTime,
                AccessTokenHash = userTokenViewModel.AccessTokenHash,
                AccessTokenExpiresDateTime = userTokenViewModel.AccessTokenExpiresDateTime
            };
        }
    }
}
