using System;

namespace SaeedRezayi.DomainClasses.Authentication
{
    public class UserTokenInfo
    {
        public int Id { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }
    }
}