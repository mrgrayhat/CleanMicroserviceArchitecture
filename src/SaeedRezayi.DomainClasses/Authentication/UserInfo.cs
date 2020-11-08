using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SaeedRezayi.DomainClasses.Authentication.JoiningTables;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Authentication
{
    public class UserInfo
    {
        public UserInfo()
        {
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserTokenInfo>();
            UserFriends = new HashSet<UserFriendsInfo>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string DisplayName { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string Tell { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Birthday { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        public DateTimeOffset? LastLoggedIn { get; set; }
        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public int CultureId { get; set; }
        public virtual CultureInfo Culture { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserTokenInfo> UserTokens { get; set; }
        public virtual ICollection<UserFriendsInfo> UserFriends { get; set; }
    }
}
