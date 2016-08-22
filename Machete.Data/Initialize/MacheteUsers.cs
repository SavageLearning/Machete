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

            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(DB));
            var user = new ApplicationUser()
            {
                UserName = "jadmin",
                IsApproved = true,
                Email = "here@there.org"
            };

            ir = um.Create(user, "ChangeMe");
            ir = um.AddToRole(user.Id, "Administrator"); //Default Administrator, edit to change
            ir = um.AddToRole(user.Id, "Teacher"); //Required to make tests work
            DB.Commit();
        }

    }
}
