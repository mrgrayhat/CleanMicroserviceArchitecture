namespace SaeedRezayi.DomainClasses.Authentication.JoiningTables
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual UserInfo User { get; set; }
        public virtual RoleInfo Role { get; set; }
    }
}