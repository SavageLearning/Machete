#region COPYRIGHT
// File:     ActivityController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Machete.Data;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly MacheteContext _context;
        private const int PasswordExpirationInMonths = 6; // represents number of months where users passwords expire 

        public AccountController(
            UserManager<MacheteUser> userManager,
            SignInManager<MacheteUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            MacheteContext context
        )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _context = context;
        }

        // TODO naming _
        private UserManager<MacheteUser> UserManager { get; }
        private SignInManager<MacheteUser> SignInManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }

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

            if (!User.Identity.IsAuthenticated)
                return View(new LoginViewModel { Action = "ExternalLogin", ReturnUrl = returnUrl });

            // Employers could still have a link to the old page, so redirect them
            if (User.IsInRole("Hirer")) {
                if (Url.IsLocalUrl("/V2/Onlineorders")) return Redirect("/V2/Onlineorders");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
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
                
                if (result?.Succeeded == true)
                {
                    _levent.Level = LogLevel.Info;
                    _levent.Message = "Logon successful";
                    _levent.Properties["username"] = model.UserName;
                    _logger.Log(_levent);
                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl); // V2
                    return RedirectToAction("Index", "Home");
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
            bool isExpired = false;
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(username);
                if (user != null)
                    isExpired = user.LastPasswordChangedDate <= DateTime.Today.AddMonths(-PasswordExpirationInMonths);
            }

            return Json(new { pwdExpired = isExpired });
        }

        [AllowAnonymous]
        public async Task<JsonResult> ChangeExpiredPasswordAsync(string username, string password, string newpassword)
        {
            var message = "Could not update password";
            var status = false;

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await UserManager.ChangePasswordAsync(user, password, newpassword);
                    if (result.Succeeded)
                    {
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

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private Guid GetApplicationId() => _context.Users
            .Select(p => p.ApplicationId)
            .First();

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

        // GET: /Account/Manage
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            switch (message) {
                case ManageMessageId.ChangePasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been changed.";
                    break;
                case ManageMessageId.SetPasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been set.";
                    break;
                case ManageMessageId.RemoveLoginSuccess:
                    ViewBag.StatusMessage = "The external login was removed.";
                    break;
                case ManageMessageId.Error:
                    ViewBag.StatusMessage = "An error has occurred.";
                    break;
                default:
                    ViewBag.StatusMessage = "";
                    break;
            }

            var user = await UserManager.GetUserAsync(HttpContext.User);
            if (user == null) return StatusCode(404);
            ViewBag.HasLocalPassword = user.PasswordHash != null;
            ViewBag.ReturnUrl = Url.Action("Manage");

            var model = new ManageUserViewModel { Action = "LinkLogin", ReturnUrl = "Manage" };
            
            ModelState["OldPassword"]?.Errors.Clear();
            ModelState["NewPassword"]?.Errors.Clear();
            
            return View(model);
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, Teacher")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var hasPassword = user.PasswordHash != null;
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            IdentityResult result;
            ManageMessageId success;
            
            if (hasPassword)
            {
                result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                user.LastPasswordChangedDate = DateTime.Today;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                success = ManageMessageId.ChangePasswordSuccess;
            } else {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                var state = ModelState["OldPassword"];
                state?.Errors.Clear();
                result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                success = ManageMessageId.SetPasswordSuccess;
            }

            if (result.Succeeded) {
                return RedirectToAction("Manage", new { Message = success });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/Edit
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(string id)
        {
            var user = _context.Users.First(u => u.Id == id);
            if (user == null) return StatusCode(404);
            return View(new EditUserViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.First(u => u.Id == model.Id);
                var name = model.FirstName.Trim() + "." + model.LastName.Trim();
                user.UserName = name;
                user.LoweredUserName = name.ToLower();
                user.Email = model.Email.Trim();
                user.LoweredEmail = model.Email.Trim().ToLower();
                user.IsApproved = model.IsApproved;
                user.IsLockedOut = model.IsLockedOut;
                var state = ModelState["NewPassword"];
                bool changePassword = !string.IsNullOrEmpty(state.AttemptedValue);
                bool hasPassword = user.PasswordHash != null;
                
                // TODO this is unnecessarily complicated
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
            if (user == null) return StatusCode(404);
            return View(new EditUserViewModel(user));
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
            var userBeingModified = _context.Users.First(u => u.Id == id);
            List<IdentityRole> allRoles = RoleManager.Roles.ToList();
            return View(new SelectUserRolesViewModel(userBeingModified, allRoles, UserManager));
        }

        // note to self 2019-01-18; this part is working, don't touch it.
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoles(SelectUserRolesViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var userBeingModified = await UserManager.FindByNameAsync(model.UserName);
            
            foreach (var role in model.Roles)
            {
                // Only administrators can provide administrator access
                if (!User.IsInRole("Administrator") && role.RoleName == "Administrator")
                    continue;

                bool userIsInRole = await UserManager.IsInRoleAsync(userBeingModified, role.RoleName);
                
                if ((role.Selected && userIsInRole) || (!role.Selected && !userIsInRole)) // then we don't care
                    continue;

                var result = new IdentityResult();
                
                if (role.Selected && !userIsInRole)
                    result = await UserManager.AddToRoleAsync(userBeingModified, role.RoleName);
                
                if (!role.Selected && userIsInRole)
                    result = await UserManager.RemoveFromRoleAsync(userBeingModified, role.RoleName);
                
                if (!result.Succeeded)
                {
                    throw new Exception("AccountController, UserRoles method, `result` failed: " + result.Errors);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Account/LogOff
        [AllowAnonymous]
        public async Task<ActionResult> LogOff()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }
    }
}
