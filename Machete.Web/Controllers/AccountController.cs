
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Machete.Data;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using NLog;
using DbFunctions = Machete.Service.DbFunctions;

namespace Machete.Web.Controllers
{
    [Authorize]
    [ElmahHandleError]
    public class AccountController : MacheteController
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly LogEventInfo _levent = new LogEventInfo(LogLevel.Debug, "AccountController", "");
        private UserManager<MacheteUser> UserManager { get; set; }
        private SignInManager<MacheteUser> SignInManager { get; }
        private readonly MacheteContext _context;
        private const int PasswordExpirationInMonths = 6; // this constant represents number of months where users passwords expire 
        
        private readonly IHtmlLocalizer<AccountController> _localizer;

        public AccountController(
            UserManager<MacheteUser> userManager,
            SignInManager<MacheteUser> signInManager,
            IHtmlLocalizer<AccountController> localizer,
            MacheteContext context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = context;
            _localizer = localizer;
        }

        // URL: /Account/Index
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Index()
        {
            // Hirer accounts use email addresses as username, so the list filters out usernames that are
            // email addresses because this View only exists to modify internal Machete user accounts
            var users = _context.Users;
            if (users == null)
                throw new ArgumentNullException();
            var model = users
                .Select(u => new UserSettingsViewModel {
                    ProviderUserKey = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsApproved = u.IsApproved ? "Yes" : "No",
                    IsLockedOut = u.IsLockedOut ? "Yes" : "No",
                    IsOnline = DbFunctions.DiffHours(u.LastLoginDate, DateTime.Now) < 1 ? "Yes" : "No",
                    CreationDate = u.CreateDate,
                    LastLoginDate = u.LastLoginDate
                }).Where (u => !u.UserName.Contains("@"));

            return View(model);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            //var userIdentity = new ClaimsIdentity("​​ApplicationCookie");
            if (!User.Identity.IsAuthenticated)
            {
                var model = new LoginViewModel();
                model.Action = "ExternalLogin";
                model.ReturnUrl = returnUrl;
                return View(model);
            }

            // Employers could still login to the old page, so redirect
            if (!User.IsInRole("Hirer"))
                return RedirectToAction("Index", "Home");
            return RedirectToLocal("/V2/Onlineorders");
        }

        // POST: /Account/LoginPost
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result != null && result.Succeeded)
                {
                    _levent.Level = LogLevel.Info;
                    _levent.Message = "Logon successful";
                    _levent.Properties["username"] = model.UserName;
                    _logger.Log(_levent);
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", ValidationStrings.invalidLogin);
            }

            // If we got this far, something failed, redisplay form
            _levent.Level = LogLevel.Info;
            _levent.Message = "Logon failed for " + model.UserName;
            _logger.Log(_levent);            
            return View(model);
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsPasswordExpiredAsync(string username)
        {
            var isExpired = false;
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(username);
                if (user != null)
                {
                    isExpired = user.LastPasswordChangedDate <= DateTime.Today.AddMonths(-PasswordExpirationInMonths);
                }
            }

            return Json(new {pwdExpired = isExpired});
        }

        [AllowAnonymous]
        public async Task<JsonResult> ChangeExpiredPasswordAsync(string username, string password, string newpassword)
        {
            // TODO: internationalize string
            var message = "Could not update password";
            var status = false;

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(username);//, password);
                if (user != null)
                {
                    var result = await UserManager.ChangePasswordAsync(user, password, newpassword);
                    if (result.Succeeded)
                    {
                        // TODO: internationalize string
                        message = "Password successfully updated.";
                        status = true;

                        user.LastPasswordChangedDate = DateTime.Today;
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        message = result.Errors.First().Description;
                    }
                }
            }

            return Json(new { succeeded = status, message });
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
            if (!ModelState.IsValid) return View(model);
            var newUserName = model.FirstName.Trim() + "." + model.LastName.Trim();
            var user = new MacheteUser { UserName = newUserName, LoweredUserName = newUserName.ToLower(), ApplicationId = GetApplicationId(), Email = model.Email.Trim(), LoweredEmail = model.Email.Trim() };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // TODO: provide messaging to administrator to add appropriate roles to their account
                //await SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private Guid GetApplicationId() => _context.Users
            .Select(p => p.ApplicationId)
            .First();

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey, string providerDisplayName)
        {
            ManageMessageId? message;

            var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
            var result = await UserManager.RemoveLoginAsync(user, providerDisplayName, providerKey);
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
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            
            var user = await UserManager.GetUserAsync(HttpContext.User);
            ViewBag.HasLocalPassword = HasPassword(user);
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (user == null)
            {
                return StatusCode(404);
            }
            var model = new ManageUserViewModel();
            model.Action = "LinkLogin";
            model.ReturnUrl = "Manage";
            var old = ModelState["OldPassword"];
            old?.Errors.Clear();
            var blah = ModelState["NewPassword"];
            blah?.Errors.Clear();
            return View(model);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var hasPassword = HasPassword(user);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.LastPasswordChangedDate = DateTime.Today;
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }

                    AddErrors(result);
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                var state = ModelState["OldPassword"];
                state?.Errors.Clear();

                if (!ModelState.IsValid) return View(model);
                var result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            MacheteUser user = _context.Users.First(u => u.Id == id);
            if (user == null)
            {
                return StatusCode(404);
            }

            EditUserViewModel model = new EditUserViewModel(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.First(u => u.Id == model.Id);
                string name = model.FirstName.Trim() + "." + model.LastName.Trim();
                // Update the user data:
                //user.FirstName = model.FirstName; //We can't have FirstName and
                //user.LastName = model.LastName;   //LastName without refactor
                user.UserName = name;
                user.LoweredUserName = name.ToLower();
                user.Email = model.Email.Trim();
                user.LoweredEmail = model.Email.Trim().ToLower();
                user.IsApproved = model.IsApproved;
                user.IsLockedOut = model.IsLockedOut;
                var state = ModelState["NewPassword"];
                bool changePassword = state.AttemptedValue != "";
                bool hasPassword = HasPassword(user);
                if (changePassword && hasPassword)
                {
                    var remove = await UserManager.RemovePasswordAsync(user);
                    if (remove.Succeeded)
                    {
                        var result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.LastPasswordChangedDate = DateTime.Today.AddMonths(-PasswordExpirationInMonths);
                            ViewBag.Message = "Password successfully updated.";
                        }
                        else
                        {
                            throw new MacheteIntegrityException("You have to add a password if you remove one.");
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
                else if (changePassword)
                {
                    model.ErrorMessage = "This user's password is managed by another service.";
                    model.NewPassword = "";
                    model.ConfirmPassword = "";
                    return View(model);
                }
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
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
            var user = _context.Users.First(u => u.Id == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return StatusCode(404);
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = _context.Users.First(u => u.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult UserRoles(string id)
        {
            var user = _context.Users.First(u => u.Id == id);
            var model = new SelectUserRolesViewModel(user, _context);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoles(SelectUserRolesViewModel model)
        {
            if (!ModelState.IsValid) return View();

            foreach (var role in model.Roles)
            {
                // Only administrators can provide administrator access
                var user = await UserManager.FindByNameAsync(model.UserName);
                
                bool isAdminRestricted = !User.IsInRole("Administrator") && role.RoleName == "Administrator" ? true : false;
                bool isUserInRole = await UserManager.IsInRoleAsync(user, role.RoleName);

                // If role is deselected & user is assigned to this role - remove role from user
                // TODO: provide error reporting - must be an administrator to modify administrator accounts
                if (!isAdminRestricted)
                {
                    var result = new IdentityResult();
                    if (!role.Selected && isUserInRole)
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, role.RoleName);
                    }
                    else if (role.Selected && !isUserInRole)
                    {
                        result = await UserManager.AddToRoleAsync(user, role.RoleName);
                    }
                    if (!result.Succeeded)
                    {
                        throw new Exception("AccountController, UserRoles method, `result` failed: " + result.Errors);
                    }
                }
            }
            // Display user account index view
            return RedirectToAction("index");
            // Re-display current user roles view
        }

        //
        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
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

        // Used for XSRF protection when adding external logins

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        private bool HasPassword(MacheteUser user)
        {
            // Return whether user has a password hash
            return user.PasswordHash != null;
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

            return RedirectToAction("Index", "Home");
        }

        // Change Culture (DEPRECATED)
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }
    }
}