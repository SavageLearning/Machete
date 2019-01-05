using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Data.Initialize
{
    public static class MacheteUsers
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MacheteContext>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<MacheteUser>>();

                string[] roles = {"Administrator", "Manager", "Check-in", "PhoneDesk", "Teacher", "User", "Hirer"};
                string[] adminRoles = {"Administrator", "Teacher", "User"};

                var macheteUsers = new List<MacheteUser>
                {
                    new MacheteUser
                    {
                        UserName = "jadmin",
                        IsApproved = true,
                        Email = "jciispam@gmail.com"
                    },
                    new MacheteUser
                    {
                        UserName = "juser",
                        IsApproved = true,
                        Email = "user@there.org"
                    }
                };

                foreach (var role in roles)
                    await roleManager.CreateAsync(new IdentityRole(role));

                foreach (var user in macheteUsers)
                {
                    var hasher = new PasswordHasher<MacheteUser>();
                    user.PasswordHash = hasher.HashPassword(user, "ChangeMe");
                    await userManager.CreateAsync(user);
                }

                var adminUser = await userManager.FindByEmailAsync("jciispam@gmail.com");
                var regularUser = await userManager.FindByEmailAsync("user@there.org");

                await userManager.AddToRolesAsync(adminUser, adminRoles);

                await context.SaveChangesAsync();
            }
        }
    }
}
