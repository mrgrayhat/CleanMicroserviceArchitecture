using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DNTPersianUtils.Core;
using SaeedRezayi.Common.MultiLingual;

namespace SaeedRezayi.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [ValidIranianPhoneNumber]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public LanguageCodes LanguageCode { get; set; } = LanguageCodes.FA_IR;
        [Required]
        public bool AcceptTermOfUse { get; set; } = true;

    }
}
