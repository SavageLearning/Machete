using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Machete.Domain;

namespace Machete.Data
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
            
            this.Roles = new JoinCollectionFacade<IdentityRole, JoinMacheteUserIdentityRole>(
                IdentityUserRoles,
                iur => iur.IdentityRole,
                role => new JoinMacheteUserIdentityRole { MacheteUser = this, IdentityRole = role }
            );
            
            this.Logins = new JoinCollectionFacade<UserLoginInfo, JoinMacheteUserIdentityUserLoginInfo>(
                IdentityUserLogins,
                iul => iul.UserLoginInfo,
                login => new JoinMacheteUserIdentityUserLoginInfo { MacheteUser = this, UserLoginInfo = login }
            );
        }

        private ICollection<JoinMacheteUserIdentityRole> IdentityUserRoles { get; }
                 = new List<JoinMacheteUserIdentityRole>();
        [NotMapped] public ICollection<IdentityRole> Roles { get; }

        private ICollection<JoinMacheteUserIdentityUserLoginInfo> IdentityUserLogins { get; }
                 = new List<JoinMacheteUserIdentityUserLoginInfo>();
        [NotMapped] public ICollection<UserLoginInfo> Logins { get; }


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
            set { _email = value; NormalizedEmail = value; }
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
    }

    public class JoinMacheteUserIdentityUserLoginInfo
    {
        public MacheteUser MacheteUser { get; set; }
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class JoinMacheteUserIdentityRole : IdentityUserRole<Guid>
    {
        public MacheteUser MacheteUser { get; set; }
        public IdentityRole IdentityRole { get; set; }
    }
}
