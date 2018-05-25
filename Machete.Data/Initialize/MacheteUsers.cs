using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Machete.Data
{
    public static class MacheteUsers
    {

        public static void Initialize(MacheteContext DB)
        {
            IdentityResult ir;

            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(DB));
            ir = rm.Create(new IdentityRole("Administrator"));
            ir = rm.Create(new IdentityRole("Manager"));
            ir = rm.Create(new IdentityRole("Check-in"));
            ir = rm.Create(new IdentityRole("PhoneDesk"));
            ir = rm.Create(new IdentityRole("Teacher"));
            ir = rm.Create(new IdentityRole("User"));
            ir = rm.Create(new IdentityRole("Hirer")); // This role is used exclusively for the online hiring interface

            var um = new UserManager<MacheteUser>(
                new UserStore<MacheteUser>(DB));
            var admin = new MacheteUser()
            {
                UserName = "jadmin",
                IsApproved = true,
                Email = "jciispam@gmail.com"
            };
            var user = new MacheteUser()
            {
                UserName = "juser",
                IsApproved = true,
                Email = "user@there.org"
            };
            ir = um.Create(admin, "ChangeMe");
            ir = um.AddToRole(admin.Id, "Administrator"); //Default Administrator, edit to change
            ir = um.AddToRole(admin.Id, "Teacher"); //Required to make tests work
            ir = um.Create(user, "ChangeMe");
            ir = um.AddToRole(admin.Id, "User"); //Default Administrator, edit to change
            DB.Commit();
        }

    }
}
