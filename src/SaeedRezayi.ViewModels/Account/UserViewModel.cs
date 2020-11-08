using System;
using System.Linq;
using Newtonsoft.Json;
using DNTPersianUtils.Core;
using System.Collections.Generic;
using SaeedRezayi.DomainClasses.Common;
using System.ComponentModel.DataAnnotations;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.ViewModels.Account
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            //UserRoles = new HashSet<UserRoleViewModel>();
            //UserTokens = new HashSet<UserTokenViewModel>();
            UserFriends = new HashSet<UserViewModel>();
        }

        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [ValidIranianMobileNumber]
        public string Tell { get; set; }
        [Url]
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }
        [DataType(DataType.Text)]
        public string Address { get; set; }
        public string Country { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public string Bio { get; set; }

        [DataType(DataType.Date)]
        public string Birthday { get; set; }
        public int CultureId { get; set; } = 1;
        public virtual CultureInfo Culture { get; set; }
        //public virtual ICollection<UserRoleViewModel> UserRoles { get; set; }

        //public virtual ICollection<UserTokenViewModel> UserTokens { get; set; }
        public virtual ICollection<UserViewModel> UserFriends { get; set; }

        public static implicit operator UserViewModel(UserInfo user)
        {
            if (user == null)
                return null;

            return new UserViewModel
            {
                Id = user.Id,
                Culture = user.Culture,
                CultureId = user.CultureId,
                Username = user.Username,
                Password = user.Password,
                DisplayName = user.DisplayName,
                IsActive = user.IsActive,
                LastLoggedIn = user.LastLoggedIn,
                SerialNumber = user.SerialNumber,
                Bio = user.Bio,
                Birthday = user.Birthday,
                Country = user.Country,
                Address = user.Address,
                EmailAddress = user.EmailAddress,
                ProfilePicture = user.ProfilePicture,
                Tell = user.Tell,
                WebsiteUrl = user.WebsiteUrl,
                UserFriends = user.UserFriends?.Select(x => (UserViewModel)x.User2).ToList()
            };
        }
        public static implicit operator UserInfo(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                return null;

            return new UserInfo
            {
                Id = userViewModel.Id,
                Culture = userViewModel.Culture,
                CultureId = userViewModel.CultureId,
                DisplayName = userViewModel.DisplayName,
                IsActive = userViewModel.IsActive,
                LastLoggedIn = userViewModel.LastLoggedIn,
                Password = userViewModel.Password,
                SerialNumber = userViewModel.SerialNumber,
                Username = userViewModel.Username,
                Bio = userViewModel.Bio,
                Birthday = userViewModel.Birthday,
                Country = userViewModel.Country,
                Address = userViewModel.Address,
                EmailAddress = userViewModel.EmailAddress,
                ProfilePicture = userViewModel.ProfilePicture,
                Tell = userViewModel.Tell,
                WebsiteUrl = userViewModel.WebsiteUrl

            };
        }
    }
}
