using System.Collections.Generic;
using SaeedRezayi.DomainClasses.Authentication.JoiningTables;

namespace SaeedRezayi.DomainClasses.Authentication
{
    public class RoleInfo
    {
        public RoleInfo()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}