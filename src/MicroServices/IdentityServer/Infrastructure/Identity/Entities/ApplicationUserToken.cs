using System;
using Microsoft.AspNetCore.Identity;

namespace STS.Infrastructure.Identity.Entities
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}