using BookLendingSystem.Application.Common;
using BookLendingSystem.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Infrastructure.Seed
{
    public static class SeedAdmin
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(SettingAdmin.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = SettingAdmin.UserName,
                    Email = SettingAdmin.Email,
                };


                var result = await userManager.CreateAsync(user, SettingAdmin.Password);

                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(user, SettingAdmin.Role);
                }
                else
                {
                    
                    var roles = await userManager.GetRolesAsync(user);
                    if (!roles.Contains(SettingAdmin.Role))
                    {
                        await userManager.AddToRoleAsync(user, SettingAdmin.Role);
                    }
                }
                }
        }
    }
}
       
    
