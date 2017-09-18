using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Extensions;
using Machete.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;

namespace Machete.Identity
{

    public class UserService : UserServiceBase
    {
        public bool EnableSecurityStamp { get; set; }
        public string DisplayNameClaimType { get; set; }
        protected readonly UserManager userManager;

        public UserService(UserManager userMgr)
        {
            this.userManager = userMgr;
            EnableSecurityStamp = true;
        }

        private async Task<IEnumerable<Claim>> GetClaimsFromAccount(MacheteUser user)
        {
            var claims = new List<Claim>{
                new Claim(Constants.ClaimTypes.Subject, user.Id.ToString()),
                new Claim(Constants.ClaimTypes.PreferredUserName, user.UserName),
            };

            if (userManager.SupportsUserEmail)
            {
                var email = await userManager.GetEmailAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(email))
                {
                    claims.Add(new Claim(Constants.ClaimTypes.Email, email));
                    var verified = await userManager.IsEmailConfirmedAsync(user.Id);
                    claims.Add(new Claim(Constants.ClaimTypes.EmailVerified, verified ? "true" : "false"));
                }
            }

            if (userManager.SupportsUserPhoneNumber)
            {
                var phone = await userManager.GetPhoneNumberAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(phone))
                {
                    claims.Add(new Claim(Constants.ClaimTypes.PhoneNumber, phone));
                    var verified = await userManager.IsPhoneNumberConfirmedAsync(user.Id);
                    claims.Add(new Claim(Constants.ClaimTypes.PhoneNumberVerified, verified ? "true" : "false"));
                }
            }

            if (userManager.SupportsUserClaim)
            {
                claims.AddRange(await userManager.GetClaimsAsync(user.Id));
            }

            if (userManager.SupportsUserRole)
            {
                var roleClaims =
                    from role in await userManager.GetRolesAsync(user.Id)
                    select new Claim(Constants.ClaimTypes.Role, role);
                claims.AddRange(roleClaims);
            }

            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                claims.Add(new Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                claims.Add(new Claim("family_name", user.LastName));
            }
            return claims;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext ctx)
        {
            ClaimsPrincipal subject = ctx.Subject;
            var requestedClaimTypes = ctx.RequestedClaimTypes;

            if (subject == null) throw new ArgumentNullException("subject");

            string key = subject.GetSubjectId();
            var acct = await userManager.FindByIdAsync(key);
            if (acct == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var claims = await GetClaimsFromAccount(acct);
            if (requestedClaimTypes != null && requestedClaimTypes.Any())
            {
                claims = claims.Where(x => requestedClaimTypes.Contains(x.Type));
            }

            ctx.IssuedClaims = claims;
        }


        private async Task<string> GetDisplayNameForAccountAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            var claims = await GetClaimsFromAccount(user);

            Claim nameClaim = null;
            if (DisplayNameClaimType != null)
            {
                nameClaim = claims.FirstOrDefault(x => x.Type == DisplayNameClaimType);
            }
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (nameClaim != null) return nameClaim.Value;

            return user.UserName;
        }

        private async Task<MacheteUser> FindUserAsync(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        private Task<AuthenticateResult> PostAuthenticateLocalAsync(MacheteUser user, SignInMessage message)
        {
            return Task.FromResult<AuthenticateResult>(null);
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext ctx)
        {
            var username = ctx.UserName;
            var password = ctx.Password;
            var message = ctx.SignInMessage;

            ctx.AuthenticateResult = null;

            if (userManager.SupportsUserPassword)
            {
                var user = await FindUserAsync(username);
                if (user != null)
                {
                    if (userManager.SupportsUserLockout &&
                        await userManager.IsLockedOutAsync(user.Id))
                    {
                        return;
                    }

                    if (await userManager.CheckPasswordAsync(user, password))
                    {
                        if (userManager.SupportsUserLockout)
                        {
                            await userManager.ResetAccessFailedCountAsync(user.Id);
                        }

                        var result = await PostAuthenticateLocalAsync(user, message);
                        if (result == null)
                        {
                            var claims = await GetClaimsForAuthenticateResult(user);
                            result = new AuthenticateResult(user.Id.ToString(), await GetDisplayNameForAccountAsync(user.Id), claims);
                        }

                        ctx.AuthenticateResult = result;
                    }
                    else if (userManager.SupportsUserLockout)
                    {
                        await userManager.AccessFailedAsync(user.Id);
                    }
                }
            }
        }

        private async Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(MacheteUser user)
        {
            List<Claim> claims = new List<Claim>();
            if (EnableSecurityStamp && userManager.SupportsUserSecurityStamp)
            {
                var stamp = await userManager.GetSecurityStampAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(stamp))
                {
                    claims.Add(new Claim("security_stamp", stamp));
                }
            }
            return claims;
        }

        public override async Task AuthenticateExternalAsync(ExternalAuthenticationContext ctx)
        {
            var externalUser = ctx.ExternalIdentity;
            var message = ctx.SignInMessage;

            if (externalUser == null)
            {
                throw new ArgumentNullException("externalUser");
            }

            var user = await userManager.FindAsync(new Microsoft.AspNet.Identity.UserLoginInfo(externalUser.Provider, externalUser.ProviderId));
            if (user == null)
            {
                ctx.AuthenticateResult = await ProcessNewExternalAccountAsync(externalUser.Provider, externalUser.ProviderId, externalUser.Claims);
            }
            else
            {
                ctx.AuthenticateResult = await ProcessExistingExternalAccountAsync(user.Id, externalUser.Provider, externalUser.ProviderId, externalUser.Claims);
            }
        }

        private async Task<AuthenticateResult> ProcessNewExternalAccountAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            var user = await TryGetExistingUserFromExternalProviderClaimsAsync(provider, claims);
            if (user == null)
            {
                user = await InstantiateNewUserFromExternalProviderAsync(provider, providerId, claims);
                if (user == null)
                    throw new InvalidOperationException("CreateNewAccountFromExternalProvider returned null");

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return new AuthenticateResult(createResult.Errors.First());
                }
            }

            var externalLogin = new Microsoft.AspNet.Identity.UserLoginInfo(provider, providerId);
            var addExternalResult = await userManager.AddLoginAsync(user.Id, externalLogin);
            if (!addExternalResult.Succeeded)
            {
                return new AuthenticateResult(addExternalResult.Errors.First());
            }

            var result = await AccountCreatedFromExternalProviderAsync(user.Id, provider, providerId, claims);
            if (result != null) return result;

            return await SignInFromExternalProviderAsync(user.Id, provider);
        }

        private Task<MacheteUser> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Email);
            var name = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
            string username;

            if (email != null)
                username = email.Value;
            else if (name != null)
                username = name.Value;
            else
                username = "unknown user";

            var user = new MacheteUser()
            {
                UserName = username,
                LoweredUserName = username.ToLower(),
                Email = email.Value,
                EmailConfirmed = true,
                LoweredEmail = email.Value.ToLower()
            };
            return Task.FromResult(user);
        }

        private Task<MacheteUser> TryGetExistingUserFromExternalProviderClaimsAsync(string provider, IEnumerable<Claim> claims)
        {
            return Task.FromResult<MacheteUser>(null);
        }

        private async Task<AuthenticateResult> AccountCreatedFromExternalProviderAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            //claims = await SetAccountEmailAsync(userID, claims);
            //claims = await SetAccountPhoneAsync(userID, claims);
            List<Claim> newClaims = claims.ToList();
            newClaims.Add(new Claim(Constants.ClaimTypes.Role, "Hirer"));
            return await UpdateAccountFromExternalClaimsAsync(userID, provider, providerId, newClaims);
        }

        private async Task<AuthenticateResult> SignInFromExternalProviderAsync(string userID, string provider)
        {
            var user = await userManager.FindByIdAsync(userID);
            var claims = await GetClaimsForAuthenticateResult(user);

            return new AuthenticateResult(
                userID.ToString(),
                await GetDisplayNameForAccountAsync(userID),
                claims,
                authenticationMethod: Constants.AuthenticationMethods.External,
                identityProvider: provider);
        }

        private async Task<AuthenticateResult> UpdateAccountFromExternalClaimsAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            var existingClaims = await userManager.GetClaimsAsync(userID);
            var intersection = existingClaims.Intersect(claims, new ClaimComparer());
            var newClaims = claims.Except(intersection, new ClaimComparer());

            foreach (var claim in newClaims)
            {
                var result = await userManager.AddClaimAsync(userID, claim);
                if (!result.Succeeded)
                {
                    return new AuthenticateResult(result.Errors.First());
                }
            }

            return null;
        }

        private async Task<AuthenticateResult> ProcessExistingExternalAccountAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return await SignInFromExternalProviderAsync(userID, provider);
        }

        private async Task<IEnumerable<Claim>> SetAccountEmailAsync(string userID, IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Email);
            if (email != null)
            {
                var userEmail = await userManager.GetEmailAsync(userID);
                if (userEmail == null)
                {
                    // if this fails, then presumably the email is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await userManager.SetEmailAsync(userID, email.Value);
                    if (result.Succeeded)
                    {
                        var email_verified = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.EmailVerified);
                        if (email_verified != null && email_verified.Value == "true")
                        {
                            var token = await userManager.GenerateEmailConfirmationTokenAsync(userID);
                            await userManager.ConfirmEmailAsync(userID, token);
                        }

                        var emailClaims = new string[] { Constants.ClaimTypes.Email, Constants.ClaimTypes.EmailVerified };
                        return claims.Where(x => !emailClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        private async Task<IEnumerable<Claim>> SetAccountPhoneAsync(string userID, IEnumerable<Claim> claims)
        {
            var phone = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.PhoneNumber);
            if (phone != null)
            {
                var userPhone = await userManager.GetPhoneNumberAsync(userID);
                if (userPhone == null)
                {
                    // if this fails, then presumably the phone is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await userManager.SetPhoneNumberAsync(userID, phone.Value);
                    if (result.Succeeded)
                    {
                        var phone_verified = claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.PhoneNumberVerified);
                        if (phone_verified != null && phone_verified.Value == "true")
                        {
                            var token = await userManager.GenerateChangePhoneNumberTokenAsync(userID, phone.Value);
                            await userManager.ChangePhoneNumberAsync(userID, phone.Value, token);
                        }

                        var phoneClaims = new string[] { Constants.ClaimTypes.PhoneNumber, Constants.ClaimTypes.PhoneNumberVerified };
                        return claims.Where(x => !phoneClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        public override async Task IsActiveAsync(IsActiveContext ctx)
        {
            var subject = ctx.Subject;

            if (subject == null) throw new ArgumentNullException("subject");

            var id = subject.GetSubjectId();
            var acct = await userManager.FindByIdAsync(id);

            ctx.IsActive = false;

            if (acct != null)
            {
                if (EnableSecurityStamp && userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(x => x.Type == "security_stamp").Select(x => x.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await userManager.GetSecurityStampAsync(id);
                        if (db_security_stamp != security_stamp)
                        {
                            return;
                        }
                    }
                }

                ctx.IsActive = true;
            }
        }

    }
}