using System;

namespace SaeedRezayi.DomainClasses.Authentication
{
    public class UserFriendsInfo
    {
        //public int Id { get; set; }
        //public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public int User1Id { get; set; }
        public UserInfo User1 { get; set; }

        public int User2Id { get; set; }
        public UserInfo User2 { get; set; }

    }
}
