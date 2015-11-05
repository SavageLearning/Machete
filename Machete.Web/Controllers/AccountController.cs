﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Machete.Web.Models;
using System.Data.SqlClient;
using System.Configuration;
using Machete.Web.Helpers;
using NLog;
using System.Globalization;
using System.Web.Security;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Web.Routing;
using Machete.Service;
using System.Collections.ObjectModel;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;

namespace Machete.Web.Controllers
{
    [Authorize]
    [ElmahHandleError]
    public class AccountController : Controller
    {
        Logger log = LogManager.GetCurrentClassLogger();
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "AccountController", "");
        public IMyUserManager<ApplicationUser> UserManager { get; private set; }
        private readonly IDatabaseFactory DatabaseFactory;
        private CultureInfo CI;
        private const int PASSWORD_EXPIRATION_IN_MONTHS = 6; // this constant represents number of months where users passwords expire 

        public AccountController(IMyUserManager<ApplicationUser> userManager, IDatabaseFactory databaseFactory)
        {
            UserManager = userManager;
            DatabaseFactory = databaseFactory;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }

        // GET: /Account/Index
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Index()
        {
            // Retrieve users 
            // Note: Hirer accounts use email addresses as username, so the list filters out usernames that are email addresses
            // This display is only to modify internal Machete user accounts (not to modify employer accounts)
            IDbSet<ApplicationUser> users = DatabaseFactory.Get().Users;
            if (users == null)
            {
                // TODO: throw alert
            }
            IQueryable<UserSettingsViewModel> model = users
                    .Select(u => new UserSettingsViewModel {
                            ProviderUserKey = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            IsApproved = u.IsApproved ? "Yes" : "No",
                            IsLockedOut = u.IsLockedOut ? "Yes" : "No",
                            IsOnline = (DbFunctions.DiffHours(u.LastLoginDate, DateTime.Now) < 1) ? "Yes" : "No",
                            CreationDate = u.CreateDate,
                            LastLoginDate = u.LastLoginDate
                        })
                        .Where (u => !u.UserName.Contains("@"));

            // TODO: consider messaging user if no users were found

            return View(model);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Hirer"))
                {
                    return RedirectToAction("Index", "HirerWorkOrder");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (returnUrl != null && returnUrl != "")
            {
                // Redirect users back to external signin
                string[] controller = returnUrl.Split(new Char[] {'/'});
                if (controller.Length >= 2 && (controller[1] == "HirerWorkOrder" || controller[1] == "HirerAccount"))
                {
                    return RedirectToAction("Login", "HirerAccount");
                }
            }

            LoginViewModel model = new LoginViewModel();
            model.Action = "ExternalLogin";
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        // TODO: Consider changing name to LoginAsync for naming convention
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    // Log successful log on
                    levent.Level = LogLevel.Info; 
                    levent.Message = "Logon successful";
                    levent.Properties["username"] = model.UserName; 
                    log.Log(levent);

                    await SignInAsync(user, model.RememberMe);
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", Machete.Web.Resources.ValidationStrings.invalidLogin);
                }
            }

            // If we got this far, something failed, log event & redisplay form
            levent.Level = LogLevel.Info; 
            levent.Message = "Logon failed for " + model.UserName;
            log.Log(levent);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsPasswordExpiredAsync(string username, string password)
        {
            bool isExpired = false;
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(username, password);
                if (user != null)
                {
                    isExpired = (user.LastPasswordChangedDate <= DateTime.Today.AddMonths(-PASSWORD_EXPIRATION_IN_MONTHS));
                }
            }

            return Json(new { pwdExpired = isExpired }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<JsonResult> ChangeExpiredPasswordAsync(string username, string password, string newpassword)
        {
            // TODO: internationalize string
            string message = "Could not update password";
            bool status = false;

            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(username, password);
                if (user != null)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, password, newpassword);
                    if (result.Succeeded)
                    {
                        // TODO: internationalize string
                        message = "Password successfully updated.";
                        status = true;

                        user.LastPasswordChangedDate = DateTime.Today;
                        MacheteContext Db = DatabaseFactory.Get();
                        Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        await Db.SaveChangesAsync();
                    }
                    else
                    {
                        message = result.Errors.First();
                    }
                }
            }

            return Json(new { succeeded = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string newUserName = model.FirstName.Trim() + "." + model.LastName.Trim();
                ApplicationUser user = new ApplicationUser() { UserName = newUserName, LoweredUserName = newUserName.ToLower(), ApplicationId = GetApplicationID(), Email = model.Email.Trim(), LoweredEmail = model.Email.Trim() };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // TODO: add user role to user & sign them in
                    // TODO: provide messaging to administrator to add appropriate roles to their account
                    //await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private Guid GetApplicationID()
        {
            Guid appId = DatabaseFactory.Get().Users
                .Select(p => p.ApplicationId)
                .First();
            return appId;
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        // Manage (update) account of current signed-in user
        // GET: /Account/Manage
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public ActionResult Manage(ManageMessageId? message)
        {
            MacheteContext Db = DatabaseFactory.Get();
            string currentUserId = User.Identity.GetUserId();
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            // Retrieve user record
            ApplicationUser user = Db.Users.First(x => x.Id == currentUserId);

            ViewBag.HasLocalPassword = HasPassword(user);
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (user == null)
            {
                return HttpNotFound();
            }

            ManageUserViewModel model = new ManageUserViewModel();
            model.Action = "LinkLogin";
            model.ReturnUrl = "Manage";
            ModelState oldPassword = ModelState["OldPassword"];
            if (oldPassword != null)
            {
                oldPassword.Errors.Clear();
            }
            ModelState newPassword = ModelState["NewPassword"];
            if (newPassword != null)
            {
                newPassword.Errors.Clear();
            }
            return View(model);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            MacheteContext Db = DatabaseFactory.Get();
            string currentUserId = User.Identity.GetUserId();
            // Retrieve signed-in user record
            ApplicationUser user = Db.Users.First(x => x.Id == currentUserId);

            bool hasPassword = HasPassword(user);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.LastPasswordChangedDate = DateTime.Today;
                        Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        await Db.SaveChangesAsync();
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            ApplicationUser user = DatabaseFactory.Get().Users.First(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            EditUserViewModel model = new EditUserViewModel(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            MacheteContext Db = DatabaseFactory.Get();

            if (ModelState.IsValid)
            {
                ApplicationUser user = Db.Users.First(u => u.Id == model.Id);
                string name = model.FirstName.Trim() + "." + model.LastName.Trim();
                // Update the user data:
                user.UserName = name;
                user.LoweredUserName = name.ToLower();
                user.Email = model.Email.Trim();
                user.LoweredEmail = model.Email.Trim().ToLower();
                user.IsApproved = model.IsApproved;
                user.IsLockedOut = model.IsLockedOut;
                ModelState state = ModelState["NewPassword"];
                bool changePassword = state.Value.AttemptedValue == "" ? false : true;
                bool hasPassword = HasPassword(user);
                if (changePassword && hasPassword)
                {
                    IdentityResult remove = await UserManager.RemovePasswordAsync(user.Id);
                    if (remove.Succeeded)
                    {
                        IdentityResult result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.LastPasswordChangedDate = DateTime.Today.AddMonths(-PASSWORD_EXPIRATION_IN_MONTHS);
                            ViewBag.Message = "Password successfully updated.";
                        }
                        else
                        {
                            throw new MacheteIntegrityException("HELL to the no. You have to ADD a password if you remove one.");
                        }
                    }
                    else
                    {
                        model.ErrorMessage = "Something went wrong with your password request. We should really learn to test our software. Sorry!";
                        model.NewPassword = "";
                        model.ConfirmPassword = "";
                        return View(model);
                    }
                }
                else if (changePassword && !hasPassword)
                {
                    model.ErrorMessage = "This user's password is managed by another service.";
                    model.NewPassword = "";
                    model.ConfirmPassword = "";
                    return View(model);
                }
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            model.ErrorMessage = "Passwords must match.";
            model.NewPassword = "";
            model.ConfirmPassword = "";
            return View(model);
        }

        // GET: Account/Delete
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(string id = null)
        {
            ApplicationUser user = DatabaseFactory.Get().Users.First(u => u.Id == id);
            EditUserViewModel model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteConfirmed(string id)
        {
            MacheteContext Db = DatabaseFactory.Get();

            // Retrieve user
            ApplicationUser user = Db.Users.First(u => u.Id == id);

            // Delete user
            Db.Users.Remove(user);

            // Commit deletion to db
            int result = Db.SaveChanges();
            if (result == 0)
            {
                // TODO: provide alert - user wasn't deleted
            }
            else if (result > 1)
            {
                // TODO: provide alert - more than 1 user was deleted
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult UserRoles(string id)
        {
            // Retrieve User
            ApplicationUser user = DatabaseFactory.Get().Users.First(u => u.Id == id);

            // Load model data for view
            SelectUserRolesViewModel model = new SelectUserRolesViewModel(user, DatabaseFactory);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;

                foreach (SelectRoleEditorViewModel role in model.Roles)
                {
                    // Only administrators can provide administrator access
                    bool isAdminRestricted = !User.IsInRole("Administrator") && role.RoleName == "Administrator" ? true : false;
                    bool isUserInRole = await UserManager.IsInRoleAsync(model.UserId, role.RoleName);

                    // If role is deselected & user is assigned to this role - remove role from user
                    if (isAdminRestricted)
                    {
                        // TODO: provide error reporting - must be an administrator to modify administrator accounts
                    }
                    else if (!role.Selected && isUserInRole)
                    {
                        result = await UserManager.RemoveFromRoleAsync(model.UserId, role.RoleName);
                        if (!result.Succeeded)
                        {
                            // TODO: provide error reporting
                        }
                    }
                    else if (role.Selected && !isUserInRole)
                    {
                        result = await UserManager.AddToRoleAsync(model.UserId, role.RoleName);
                        if (!result.Succeeded)
                        {
                            // TODO: provide error reporting
                        }
                    }
                    else
                    {
                        // TODO: provide error reporting
                    }
                }
                // Display user account index view
                return RedirectToAction("index");
            }
            // Re-display current user roles view
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            ApplicationUser user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { FirstName = "", LastName = "" });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                ApplicationUser user = new ApplicationUser() { UserName = model.FirstName + "." + model.LastName };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/Signup
        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View("Signup");
        }

        //
        // GET: /Account/Signup
        [AllowAnonymous]
        public ActionResult HirerSignon()
        {
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = DatabaseFactory.Get().Users.First(x => x.Id == id);
            ICollection<UserLoginInfo> linkedAccounts = new Collection<UserLoginInfo>
                (
                    user.Logins
                        .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)
                            {
                                LoginProvider = x.LoginProvider,
                                ProviderKey = x.ProviderKey
                            })
                        .ToList()
                );
            ViewBag.ShowRemoveButton = HasPassword(user) || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            ClaimsIdentity identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool IsInternalUser(ApplicationUser user)
        {
            // Return whether user has a password hash
            return (user.UserName.Contains("@"));
        }

        private bool HasPassword(ApplicationUser user)
        {
            // Return whether user has a password hash
            return (user.PasswordHash != null);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                AuthenticationProperties properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        // **************************************
        // Change Culture
        // **************************************
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
            // TODO: add user input validation to prevent setting unsupported language
            // TODO: preserve field text when language changes
        }
    }
}