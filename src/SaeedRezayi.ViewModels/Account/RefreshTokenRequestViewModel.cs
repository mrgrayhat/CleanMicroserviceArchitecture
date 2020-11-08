using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SaeedRezayi.ViewModels.Account
{
    public class RefreshTokenRequestViewModel
    {
        [JsonPropertyName("refreshToken")]
        [Required]
        public string RefreshToken { get; set; }
    }
}