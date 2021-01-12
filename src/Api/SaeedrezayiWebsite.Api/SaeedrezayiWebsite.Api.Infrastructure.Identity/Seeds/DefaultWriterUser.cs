using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SaeedrezayiWebsite.Api.Application.Enums;
using SaeedrezayiWebsite.Api.Infrastructure.Identity.Models;

namespace SaeedrezayiWebsite.Api.Infrastructure.Identity.Seeds
{
    public static class DefaultWriterUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var writerUser = new ApplicationUser
            {
                UserName = "writeruser",
                Email = "writeruser@gmail.com",
                FirstName = "jack",
                LastName = "london",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != writerUser.Id))
            {
                var user = await userManager.FindByEmailAsync(writerUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(writerUser, "Writeruser@1234");
                    await userManager.AddToRoleAsync(writerUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(writerUser, Roles.Writer.ToString());
                }

            }
        }
    }
}
