using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using STS.Infrastructure.Identity;
using STS.Infrastructure.Identity.Entities;

namespace STS.Web.Seed
{
    public class IdentitySeedData : IIdentitySeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentitySeedData(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Seed(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            // Seeding Users and roles
            IdentityServerDbContext appContext = scope.ServiceProvider.GetService<IdentityServerDbContext>();

            appContext.Database.Migrate();
            CreateRoles();
            CreateUsers(appContext);
        }

        private void CreateRoles()
        {
            var rolesToAdd = new List<ApplicationRole>(){
                new ApplicationRole
                {
                    Name = "Admin",
                    Description = "Role with full rights"
                },
                new ApplicationRole
                {
                    Name = "Writer",
                    Description = "Role with writer access rights"
                },
                new ApplicationRole
                {
                    Name = "User",
                    Description = "Role with limited standard user rights"
                }
            };
            foreach (var role in rolesToAdd)
            {
                if (!_roleManager.RoleExistsAsync(role.Name).Result)
                {
                    _roleManager.CreateAsync(role).Result.ToString();
                }
            }
        }

        private void CreateUsers(IdentityServerDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Mobile = "0123456789",
                    EmailConfirmed = true
                };
                _userManager.CreateAsync(adminUser, "Admin@1234").Result.ToString();
                _userManager.AddClaimAsync(adminUser,
                    new Claim(IdentityServerConstants.StandardScopes.Phone, adminUser.Mobile.ToString(), ClaimValueTypes.Integer)).Result.ToString();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync("admin@admin.com")
                    .GetAwaiter().GetResult(), "Admin").Result.ToString();

                var normalUser = new ApplicationUser
                {
                    UserName = "user@user.com",
                    Email = "user@user.com",
                    Mobile = "0123456789",
                    EmailConfirmed = true
                };
                _userManager.CreateAsync(normalUser, "User@1234").Result.ToString();
                _userManager.AddClaimAsync(adminUser,
                    new Claim(IdentityServerConstants.StandardScopes.Phone, adminUser.Mobile.ToString(), ClaimValueTypes.Integer)).Result.ToString();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync("user@user.com")
                    .GetAwaiter().GetResult(), "User").Result.ToString();
            }
        }
    }
}