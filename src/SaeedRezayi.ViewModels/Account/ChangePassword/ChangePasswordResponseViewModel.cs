
using SaeedRezayi.Common.Messages;

namespace SaeedRezayi.ViewModels.Account.ChangePassword
{
    public class ChangePasswordResponseViewModel
    {
        public bool Succeeded { get; set; }=false;
        public string Error { get; set; }
        public MessageStatusCodeTypes StatusCode { get; set; }
    }
}
