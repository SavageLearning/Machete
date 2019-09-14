using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Machete.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Machete.Data.Initialize
{
    public static class MacheteSeedUsers
    {
        public static async Task Initialize(MacheteContext db)
        {
            string[] roleNames = { "Administrator", "Manager", "Check-in", "PhoneDesk", "Teacher", "User", "Hirer" };
            string[] adminRoleNames = { "Administrator", "Teacher", "User" };
            
            var roleStore = new MacheteRoleStore(db, true);
            var userStore = new MacheteUserStore(db, true);

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

            foreach (var roleName in roleNames)
            {
                var role = new MacheteRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                // spent time on validation replication; it's not worth it

                await roleStore.SetNormalizedRoleNameAsync(role, roleName, CancellationToken.None);
                await roleStore.CreateAsync(role, CancellationToken.None);
            }

            foreach (var user in macheteUsers)
            {
                await userStore.SetSecurityStampAsync(user, CancellationToken.None);
                var hasher = new PasswordHasher<MacheteUser>();
                user.PasswordHash = hasher.HashPassword(user, "ChangeMe");
                
                // TODO lockout

                await userStore.SetNormalizedUserNameAsync(user, user.UserName.ToUpper(), CancellationToken.None);
                await userStore.SetNormalizedEmailAsync(user, user.Email, CancellationToken.None);

                await userStore.CreateAsync(user, CancellationToken.None);
            }

            var adminUser = await userStore.FindByEmailAsync("jciispam@gmail.com", CancellationToken.None);
            var regularUser = await userStore.FindByEmailAsync("user@there.org", CancellationToken.None);

            foreach (var adminRole in adminRoleNames)
                await userStore.AddToRoleAsync(adminUser, adminRole, CancellationToken.None);
        }
    }
}
