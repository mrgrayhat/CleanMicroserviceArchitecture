using System.ComponentModel.DataAnnotations;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.ViewModels.Account.Login
{
    public class LoginRequestViewModel
    {
        [StringLength(maximumLength: 20,
            MinimumLength = 4,
            ErrorMessage = "نام کاربری باید حداقل 4 حرف و حداکثر 20 حرف باشد")]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "ایمیل نامعتبر است")]
        [Required(AllowEmptyStrings = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 32,
            MinimumLength = 4,
            ErrorMessage = "کلمه عبور باید حداقل 4 حرف و حداکثر 32 حرف باشد")]
        public string Password { get; set; }


        //public static implicit operator LoginViewModel(UserInfo user)
        //{
        //    if (user == null)
        //        return null;

        //    return new LoginViewModel
        //    {
        //        Username = user.Username,
        //        Password = user.Password,
        //        Email = user.EmailAddress
        //    };
        //}
        public static implicit operator UserInfo(LoginRequestViewModel loginViewModel)
        {
            if (loginViewModel == null)
                return null;

            return new UserInfo
            {
                Username = loginViewModel.Username,
                Password = loginViewModel.Password,
                EmailAddress = loginViewModel.Email
            };
        }
    }
}
