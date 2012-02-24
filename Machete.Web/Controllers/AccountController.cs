using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Machete.Web.Models;
using System.Globalization;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        Logger log = LogManager.GetCurrentClassLogger();
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "AccountController", "");
        
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            
            base.Initialize(requestContext);
        }
        #region User/Member management
        // **************************************
        // URL: /Account/Index
        // **************************************
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Index()
        {
            int records = 0;
            MembershipUserCollection members = Membership.GetAllUsers(0, Int32.MaxValue, out records);
            return View(members);
        }
        //
        // GET: /Account/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Account/Create
        //
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(MembersModel member)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                // TODO: Error handling
                member.UserName = member.FirstName + "." + member.LastName;
                MembershipCreateStatus createStatus = MembershipService.CreateUser(member.UserName, member.Password, member.Email, member.question, member.answer);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    levent.Level = LogLevel.Info; levent.Message = "Successfully created user: " + member.UserName;
                    levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
                
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            levent.Level = LogLevel.Info; levent.Message = "Create failed for user: " + member.UserName;
            levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
            return RedirectToAction("Index");
        }
        // **************************************
        // URL: /Account/Edit
        // **************************************
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid id)
        {

            MembershipUser member = Membership.GetUser(id, false);
            //MembersModel model = new MembersModel(member);
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(member);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection, bool IsApproved, bool IsLockedOut)
        {
            // TODO:!!!! Add admin password change/reset/something
            MembershipUser member = Membership.GetUser(id, false);
            member.IsApproved = IsApproved;
            if (IsLockedOut == false)
            {
                member.UnlockUser();
            }
            if (TryUpdateModel(member))
            {   
                foreach (string role in Roles.GetAllRoles())
                {
                    if (collection[role].Contains("true"))
                    {
                        if (!Roles.IsUserInRole(member.UserName, role)) Roles.AddUserToRole(member.UserName, role);
                        levent.Level = LogLevel.Info; levent.Message = "Added " + role + " to user: " + member.UserName;
                        levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
                    }
                    else
                    {
                        if (Roles.IsUserInRole(member.UserName, role)) Roles.RemoveUserFromRole(member.UserName, role);
                        levent.Level = LogLevel.Info; levent.Message = "Removed " + role + " from user: " + member.UserName;
                        levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
                    }
                }
                Membership.UpdateUser(member);
                levent.Level = LogLevel.Info; levent.Message = "Edited user: " + member.UserName;
                levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
                return RedirectToAction("Index");
            }
            //TODO: ERror handling
            return View(member);
        }
        // **************************************
        // 
        // **************************************
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid id)
        {
            MembershipUser member = Membership.GetUser(id, false);
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(member);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            MembershipUser member = Membership.GetUser(id, false);
            bool success = Membership.DeleteUser(member.UserName);
            if (success)
            {
                levent.Level = LogLevel.Info; levent.Message = "Deleted user: " + member.UserName;
                levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
            }
            else
            {
                levent.Level = LogLevel.Info; levent.Message = "Failed to delete user: " + member.UserName;
                levent.Properties["username"] = this.User.Identity.Name; log.Log(levent);
            }
            return RedirectToAction("Index");
        }
        #endregion 

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    levent.Level = LogLevel.Info; levent.Message = "Logon successful";
                    levent.Properties["username"] = model.UserName; log.Log(levent);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            levent.Level = LogLevel.Info; levent.Message = "Logon failed for " + model.UserName;
            log.Log(levent);
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            levent.Level = LogLevel.Info; levent.Message = "Logoff for " + this.User.Identity.Name;
            log.Log(levent);
            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.FirstName + "." + model.LastName;
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.question, model.answer);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    levent.Level = LogLevel.Info; levent.Message = "Registered new user: " + model.UserName;
                    log.Log(levent);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            levent.Level = LogLevel.Info; levent.Message = "Registration failed for " + model.UserName;
            log.Log(levent);
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    levent.Level = LogLevel.Info; levent.Message = "Password changed for " + User.Identity.Name;
                    log.Log(levent);
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            levent.Level = LogLevel.Info; levent.Message = "Password change failed for " + User.Identity.Name;
            log.Log(levent);
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }
        //[Authorize]
        public ActionResult AdminChangePassword(Guid id)
        {
            MembershipUser member = Membership.GetUser(id, false);
            AdminChangePasswordModel model = new AdminChangePasswordModel();
            model.UserName = member.UserName;
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AdminChangePassword(AdminChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser member = Membership.GetUser(model.id, false);
                if (member.ChangePassword(member.ResetPassword(), model.NewPassword))
                //if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    levent.Level = LogLevel.Info; levent.Message = "Admin changed password for " + model.UserName;
                    log.Log(levent);
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Model validation error for Admin change password.");
                }
            }

            // If we got this far, something failed, redisplay form
            levent.Level = LogLevel.Info; levent.Message = "Password change failed for " + model.UserName;
            log.Log(levent);
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            // TODO: Acknowledge the successful change
            return View();
        }

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
