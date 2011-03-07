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

namespace Machete.Web.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

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
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Account/Create
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
        public ActionResult Create(MembersModel member)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                // TODO: Error handling
                MembershipCreateStatus createStatus = MembershipService.CreateUser(member.UserName, member.Password, member.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
                
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return RedirectToAction("Index");
        }
        // **************************************
        // URL: /Account/Edit
        // **************************************
        [Authorize]
        public ActionResult Edit(Guid id)
        {

            MembershipUser member = Membership.GetUser(id, false);
            //MembersModel model = new MembersModel(member);
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(member);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            // TODO:!!!! Add admin password change/reset/something
            MembershipUser member = Membership.GetUser(id, false);
            if (TryUpdateModel(member))
            {   
                foreach (String role in Roles.GetAllRoles())
                {
                    if (collection[role].Contains("true"))
                    {
                        if (!Roles.IsUserInRole(member.UserName, role)) Roles.AddUserToRole(member.UserName, role);
                    }
                    else
                    {
                        if (Roles.IsUserInRole(member.UserName, role)) Roles.RemoveUserFromRole(member.UserName, role);
                    }
                }
                Membership.UpdateUser(member);
                return RedirectToAction("Index");
            }
            //TODO: ERror handling
            return View(member);
        }
        // **************************************
        // TODO: /Account/Delete
        // **************************************
        

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
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

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
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
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
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
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
