using SaeedRezayi.DomainClasses;

namespace SaeedRezayi.ViewModels.Api
{
    public class ApiSettingsViewModel
    {
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string RefreshTokenPath { get; set; }
        public string AccessTokenObjectKey { get; set; }
        public string RefreshTokenObjectKey { get; set; }
        public string AdminRoleName { get; set; }

        //public static implicit operator ApiSettingsViewModel(ApiSettings apiSettings)
        //{
        //    return new ApiSettingsViewModel
        //    {
        //        AccessTokenObjectKey = apiSettings.AccessTokenObjectKey,
        //        AdminRoleName = apiSettings.AdminRoleName,
        //        LoginPath = apiSettings.LoginPath,
        //        LogoutPath = apiSettings.LogoutPath,
        //        RefreshTokenObjectKey = apiSettings.RefreshTokenObjectKey,
        //        RefreshTokenPath = apiSettings.RefreshTokenPath
        //    };
        //}

        //public static implicit operator ApiSettings(ApiSettingsViewModel apiSettingsViewModel)
        //{
        //    return new ApiSettings
        //    {
        //        RefreshTokenPath = apiSettingsViewModel.RefreshTokenPath,
        //        RefreshTokenObjectKey = apiSettingsViewModel.RefreshTokenObjectKey,
        //        LogoutPath = apiSettingsViewModel.LogoutPath,
        //        LoginPath = apiSettingsViewModel.LoginPath,
        //        AdminRoleName = apiSettingsViewModel.AdminRoleName,
        //        AccessTokenObjectKey = apiSettingsViewModel.AccessTokenObjectKey
        //    };
        //}
    }
}
