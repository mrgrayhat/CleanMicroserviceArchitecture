using System;
using Microsoft.AspNetCore.Identity;

namespace STS.Infrastructure.Identity.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}