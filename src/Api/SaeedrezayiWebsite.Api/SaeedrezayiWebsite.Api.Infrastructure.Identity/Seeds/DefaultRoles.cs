using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SaeedrezayiWebsite.Api.Application.Enums;
using SaeedrezayiWebsite.Api.Infrastructure.Identity.Models;

namespace SaeedrezayiWebsite.Api.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Moderator.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Basic.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Writer.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Writer.ToString()));
        }
    }
}
