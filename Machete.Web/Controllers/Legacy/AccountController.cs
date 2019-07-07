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
using System.Security.Claims;
using System.Threading.Tasks;
using Machete.Data;
using Machete.Data.Identity;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Helpers.Api;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using NLog;
using DbFunctions = Machete.Service.DbFunctions;

namespace Machete.Web.Controllers
{
    [Authorize]
        public class AccountController : MacheteController
    {
        private UserManager<MacheteUser> _userManager { get; }
        private SignInManager<MacheteUser> _signinManager { get; }
        private RoleManager<MacheteRole> _roleManager { get; }
        
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly LogEventInfo _levent = new LogEventInfo(LogLevel.Debug, "AccountController", "");
        private readonly MacheteContext _context;
        private const int PasswordExpirationInMonths = 6; // represents number of months where users passwords expire 

        public AccountController(
            UserManager<MacheteUser> userManager,
            SignInManager<MacheteUser> signinManager,
            RoleManager<MacheteRole> roleManager,
            MacheteContext context
        )
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _context = context;
        }

        // URL: /Account/Index
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Index()
        {
            List<UserSettingsViewModel> model = new List<UserSettingsViewModel>();
            // Hirer accounts use email addresses as username, so the list filters out usernames that are
            // email addresses because this View only exists to modify internal Machete user accounts
            var users = _context.Users;
            if (users == null)
                throw new ArgumentNullException();
            
            if (User.Identity.Name == "jadmin" || User.Identity.Name.Contains("ndlon"))
            {
                foreach (var user in users)
                {
                    //bool isHirer = await user.IsInRole("Hirer", _userManager);
                    model.Add(user.ToUserSettingsViewModel(false));//isHirer
                }
                
                return View(model);
            }

            foreach (var user in users)
            {
                if (user.UserName.Equals("jadmin") || user.UserName.Contains("ndlon")) continue;
                
                bool isHirer = await user.IsInRole("Hirer", _userManager);

                if (isHirer) continue;
                
                model.Add(user.ToUserSettingsViewModel(isHirer));
            }

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
                await VerifyClaimsExistFor(model.UserName);

                var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                
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
                var user = await _userManager.FindByNameAsync(username);
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
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, password, newpassword);
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
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Nobody:
                // This comment: provide messaging to administrator to add appropriate roles to their account
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

            var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            var result = await _userManager.RemoveLoginAsync(user, providerDisplayName, providerKey);
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

            var user = await _userManager.GetUserAsync(HttpContext.User);
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
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var hasPassword = user.PasswordHash != null;
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            IdentityResult result;
            ManageMessageId success;
            
            if (hasPassword)
            {
                result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                user.LastPasswordChangedDate = DateTime.Today;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                success = ManageMessageId.ChangePasswordSuccess;
            } else {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                var state = ModelState["OldPassword"];
                state?.Errors.Clear();
                result = await _userManager.AddPasswordAsync(user, model.NewPassword);
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
            var editUserViewModel = new EditUserViewModel(user);
            
            editUserViewModel.Id = id;
            
            return View(editUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit([Bind]EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.First(u => u.Id == model.Id);
                var macheteUserName = model.FirstName.Trim() + "." + model.LastName.Trim();
                user.UserName = macheteUserName;
                user.LoweredUserName = macheteUserName.ToLower();
                user.Email = model.Email.Trim();
                user.LoweredEmail = model.Email.Trim().ToLower();
                user.IsApproved = model.IsApproved;
                user.IsLockedOut = model.IsLockedOut;

                var errors = await TryChangePassword(user, ModelState["NewPassword"].AttemptedValue);

                if (!errors.Any())
                {
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                errors.ForEach(error => ModelState.AddModelError("ErrorMessage", error));
            }

            // If we got this far, something failed, redisplay form
            model.NewPassword = "";
            model.ConfirmPassword = "";
            return View(model);
        }

        [NonAction]
        private async Task<List<string>> TryChangePassword(MacheteUser user, string attemptedValue)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrEmpty(attemptedValue)) return new List<string>();

            if (string.IsNullOrEmpty(user.PasswordHash))
                errors.Add("This user's password is managed by another service.");

            var remove = await _userManager.RemovePasswordAsync(user);
            if (!remove.Succeeded)
                errors.Add("Something went wrong with your request. Contact an administrator for assistance.");

            var result = await _userManager.AddPasswordAsync(user, attemptedValue);
            if (!result.Succeeded)
                errors.Add("Something went wrong with your request. Contact an administrator for assistance.");

            if (errors.Any()) return errors;
            
            user.LastPasswordChangedDate = DateTime.Today.AddMonths(-PasswordExpirationInMonths);
            ViewBag.Message = "Password successfully updated.";

            return new List<string>();
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
            List<MacheteRole> allRoles = _roleManager.Roles.Where(role => role.NormalizedName != "HIRER").ToList();
            return View(new SelectUserRolesViewModel(userBeingModified, allRoles, _userManager));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoles(SelectUserRolesViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var userBeingModified = await _userManager.FindByNameAsync(model.UserName);
            
            foreach (var role in model.Roles)
            {
                // Only administrators can provide administrator access
                if (!User.IsInRole("Administrator") && role.RoleName == "Administrator")
                    continue;

                bool userIsInRole = await _userManager.IsInRoleAsync(userBeingModified, role.RoleName);
                
                if ((role.Selected && userIsInRole) || (!role.Selected && !userIsInRole)) // then we don't care
                    continue;

                var result = new IdentityResult();
                
                if (role.Selected && !userIsInRole)
                    result = await _userManager.AddToRoleAsync(userBeingModified, role.RoleName);
                
                if (!role.Selected && userIsInRole)
                    result = await _userManager.RemoveFromRoleAsync(userBeingModified, role.RoleName);
                
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
            await _signinManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        
        //https://www.c-sharpcorner.com/article/claim-based-and-policy-based-authorization-with-asp-net-core-2-1/
        private async Task VerifyClaimsExistFor(string username)
        {
            // They are probably using an email, but not necessarily. Either way, it should be "username" in the db.
            var user = await _userManager.FindByNameAsync(username);
            
            var claims = await _userManager.GetClaimsAsync(user);
            var claimsList = claims.Select(claim => claim.Type).ToList();
            
            if (!claimsList.Contains(CAType.nameidentifier))
                await _userManager.AddClaimAsync(user, new Claim(CAType.nameidentifier, user.Id));           
            if (!claimsList.Contains(CAType.email))
                await _userManager.AddClaimAsync(user, new Claim(CAType.email, user.Email));
            // In the above we use the user.Email regardless of UserName. TODO inform them if a discrepancy exists.
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
