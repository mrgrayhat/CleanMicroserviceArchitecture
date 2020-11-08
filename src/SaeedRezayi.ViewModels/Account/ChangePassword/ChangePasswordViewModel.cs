using System.ComponentModel.DataAnnotations;

namespace SaeedRezayi.ViewModels.Account.ChangePassword
{
    public class ChangePasswordRequestViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}