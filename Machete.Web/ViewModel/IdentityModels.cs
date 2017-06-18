using Machete.Data;
using Machete.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Web.ViewModel
{
    public interface IMacheteUserManager<TUser>
    {
        Task<TUser> FindAsync(string userName, string password);
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<IdentityResult> CreateAsync(TUser user);
        Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(string userId);
        //IList<UserLoginInfo> GetLogins(string userId);
        Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role); 
        Task<IdentityResult> AddToRoleAsync(string userId, string role); 
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<TUser> FindAsync(UserLoginInfo login);
        Task<TUser> FindByIdAsync(string userId);
        void Dispose();
    }

    public class MacheteUserManager : UserManager<MacheteUser>, IMacheteUserManager<MacheteUser>
    {
        public MacheteUserManager(IDatabaseFactory factory)
            : base(new UserStore<MacheteUser>(factory.Get()))
        {
            this.UserValidator = new UserValidator<MacheteUser>(this) 
            { 
                AllowOnlyAlphanumericUserNames = false 
            };
            this.PasswordHasher = new SQLPasswordHasher();
        }
        public class SQLPasswordHasher : PasswordHasher
        {
            public override string HashPassword(string password)
            {
                return base.HashPassword(password);
            }

            public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
            {
                string[] passwordProperties = hashedPassword.Split('|');
                if (passwordProperties.Length != 3)
                {
                    return base.VerifyHashedPassword(hashedPassword, providedPassword);
                }
                else
                {
                    string passwordHash = passwordProperties[0];
                    int passwordformat = 1;
                    string salt = passwordProperties[2];
                    if (String.Equals(EncryptPassword(providedPassword, passwordformat, salt), passwordHash, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return PasswordVerificationResult.SuccessRehashNeeded;
                    }
                    else
                    {
                        return PasswordVerificationResult.Failed;
                    }
                }
            }

            //This is copied from the existing SQL providers and is provided only for back-compat.
            private string EncryptPassword(string pass, int passwordFormat, string salt)
            {
                if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                    return pass;

                byte[] bIn = Encoding.Unicode.GetBytes(pass);
                byte[] bSalt = Convert.FromBase64String(salt);
                byte[] bRet = null;

                if (passwordFormat == 1)
                { // MembershipPasswordFormat.Hashed 
                    HashAlgorithm hm = HashAlgorithm.Create("SHA1");
                    if (hm is KeyedHashAlgorithm)
                    {
                        KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                        if (kha.Key.Length == bSalt.Length)
                        {
                            kha.Key = bSalt;
                        }
                        else if (kha.Key.Length < bSalt.Length)
                        {
                            byte[] bKey = new byte[kha.Key.Length];
                            Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                            kha.Key = bKey;
                        }
                        else
                        {
                            byte[] bKey = new byte[kha.Key.Length];
                            for (int iter = 0; iter < bKey.Length; )
                            {
                                int len = Math.Min(bSalt.Length, bKey.Length - iter);
                                Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                                iter += len;
                            }
                            kha.Key = bKey;
                        }
                        bRet = kha.ComputeHash(bIn);
                    }
                    else
                    {
                        byte[] bAll = new byte[bSalt.Length + bIn.Length];
                        Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                        Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                        bRet = hm.ComputeHash(bAll);
                    }
                }

                return Convert.ToBase64String(bRet);
            }
        }

        public IList<UserLoginInfo> GetLogins(string userId)
        {
            return this.GetLogins(userId);
        }
    }

    /* Note: only used by OldAccountController
    public class IdentityManager
    {
        IDatabaseFactory Db;
        public IdentityManager(IDatabaseFactory plop)
        {
            Db = plop;
        }

        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(Db.Get()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(Db.Get()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(Db.Get()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(Db.Get()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(Db.Get()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }
    */

}