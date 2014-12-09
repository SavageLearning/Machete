using System;
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

        // **************************************
        // URL: /Account/Index
        // **************************************
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Index()
        {
            IEnumerable<UserSettingsViewModel> model;

            var users = DatabaseFactory.Get().Users.ToList();

            model = users
                    .Select(list => new UserSettingsViewModel
                        {
                            ProviderUserKey = list.Id,
                            UserName = list.UserName,
                            Email = list.Email,
                            IsApproved = list.IsApproved ? "Yes" : "No",
                            IsLockedOut = list.IsLockedOut ? "Yes" : "No",
                            IsOnline = (list.LastLoginDate > DateTime.Now.AddHours(-1)) ? "Yes" : "No",
                            CreationDate = list.CreateDate,
                            LastLoginDate = list.LastLoginDate
                        }
                        );
            return View(model);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            model.Action = "ExternalLogin";
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    levent.Level = LogLevel.Info; levent.Message = "Logon successful";
                    levent.Properties["username"] = model.UserName; log.Log(levent);
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            levent.Level = LogLevel.Info; levent.Message = "Logon failed for " + model.UserName;
            log.Log(levent);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsPasswordExpired(string username, string password)
        {
            var isExpired = false;
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(username, password);
                if (user != null)
                {
                    isExpired = (user.LastPasswordChangedDate <= DateTime.Today.AddMonths(-6));
                }
            }

            return Json(new { pwdExpired = isExpired }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<JsonResult> ChangeExpiredPassword(string username, string password, string newpassword)
        {
            var message = "Could not update password";
            var status = false;

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(username, password);
                if (user != null)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, password, newpassword);
                    if (result.Succeeded)
                    {
                        message = "Password successfully updated.";
                        status = true;

                        user.LastPasswordChangedDate = DateTime.Today;
                        var Db = DatabaseFactory.Get();
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

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentApplicationId = GetApplicationID();
                string newUserName = model.FirstName.Trim() + "." + model.LastName.Trim();
                var user = new ApplicationUser() { UserName = newUserName, LoweredUserName = newUserName.ToLower(), ApplicationId = currentApplicationId };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
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
            var blarg = DatabaseFactory.Get().Users
                .Select(p => p.ApplicationId)
                .First();
            return blarg;
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

        //
        // GET: /Account/Manage
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public ActionResult Manage(ManageMessageId? message)
        {
            var Db = DatabaseFactory.Get();
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var id = User.Identity.GetUserId();
            var user = Db.Users.First(x => x.Id == id);
            ViewBag.HasLocalPassword = HasPassword(user);
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (user == null)
            {
                return HttpNotFound();
            }
            ManageUserViewModel model = new ManageUserViewModel();
            model.Action = "LinkLogin";
            model.ReturnUrl = "Manage";
            ModelState old = ModelState["OldPassword"];
            if (old != null) old.Errors.Clear();
            ModelState blah = ModelState["NewPassword"];
            if (blah != null) blah.Errors.Clear();
            return View(model);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            var Db = DatabaseFactory.Get();
            var id = User.Identity.GetUserId();
            var user = Db.Users.First(x => x.Id == id);
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
            var Db = DatabaseFactory.Get();
            var user = Db.Users.First(u => u.Id == id);
            var model = new EditUserViewModel(user);
            model.Id = user.Id;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            var Db = DatabaseFactory.Get();

            if (ModelState.IsValid)
            {
                var user = Db.Users.First(u => u.Id == model.Id);
                string name = model.FirstName.Trim() + "." + model.LastName.Trim();
                // Update the user data:
                //user.FirstName = model.FirstName; //We can't have FirstName and
                //user.LastName = model.LastName;   //LastName without refactor
                user.UserName = name;
                user.LoweredUserName = name.ToLower();
                user.Email = model.Email;
                user.LoweredEmail = model.Email.ToLower();
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
                            ViewBag.Message = "Password successfully updated.";
                            //return RedirectToAction("Index");
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

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(string id = null)
        {
            var Db = DatabaseFactory.Get();
            var user = Db.Users.First(u => u.Id == id);
            var model = new EditUserViewModel(user);
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
            var Db = DatabaseFactory.Get();
            var user = Db.Users.First(u => u.Id == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult UserRoles(string id)
        {
            var Db = DatabaseFactory.Get();
            var user = Db.Users.First(u => u.Id == id);
            var model = new SelectUserRolesViewModel(user, DatabaseFactory);
            model.UserId = id;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                foreach (var role in model.Roles)
                {
                    bool iNoEdit = User.IsInRole("Manager") && role.RoleName == "Administrator" ? true : false;
                    bool iEdit = await UserManager.IsInRoleAsync(user.Id, role.RoleName);
                    if (!role.Selected && !iNoEdit)
                    {
                        if (iEdit) await UserManager.RemoveFromRoleAsync(user.Id, role.RoleName);
                    }
                    else
                    {
                        if (!iEdit && !iNoEdit) await UserManager.AddToRoleAsync(user.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
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
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
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
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
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
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.FirstName + "." + model.LastName };
                var result = await UserManager.CreateAsync(user);
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
        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
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
            var Db = DatabaseFactory.Get();
            var id = User.Identity.GetUserId();
            var user = Db.Users.First(x => x.Id == id);
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
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async Task<bool> HasPassword(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPassword(ApplicationUser user)
        {
            if (user.PasswordHash != null)
            {
                return true;
            }
            return false;
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

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
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