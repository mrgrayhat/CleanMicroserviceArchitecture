using System.Collections.Generic;

namespace SaeedRezayi.ViewModels.Account
{
    public class JwtTokensDataViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenSerial { get; set; }
        public IEnumerable<System.Security.Claims.Claim> Claims { get; set; }
    }
}
