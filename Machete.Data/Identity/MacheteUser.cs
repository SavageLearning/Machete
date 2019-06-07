using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Machete.Domain;
using Microsoft.AspNetCore.Identity;

namespace Machete.Data.Identity
{
    public class MacheteUser : IdentityUser
    {
        private string _email;

        public MacheteUser()
        {
            CreateDate = DateTime.Now;
            IsApproved = false;
            LastLoginDate = DateTime.Now;
            LastActivityDate = DateTime.Now;
            LastPasswordChangedDate = DateTime.Now;
            LastLockoutDate = DateTime.Parse("1/1/1754");
            FailedPasswordAnswerAttemptWindowStart = DateTime.Parse("1/1/1754");
            FailedPasswordAttemptWindowStart = DateTime.Parse("1/1/1754");
        }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<MacheteUserRole> UserRoles { get; set; }

        public virtual IList<MacheteUserClaim> Claims { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ApplicationId { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string MobilePIN { get; set; }

        public override string Email
        {
            get => _email;
            set { _email = value; NormalizedEmail = value.ToUpper(); }
        }

        public string LoweredEmail { get; set; }
        public string LoweredUserName { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }

        public async Task<bool> IsInRole(string roleName, UserManager<MacheteUser> userManager)
        {
            return await userManager.IsInRoleAsync(this, roleName);
        }
    }
}
