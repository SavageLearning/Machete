#region COPYRIGHT
// File:     AccountModels.cs
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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Machete.Web.Resources;
using Machete.Domain;

namespace Machete.Web.Models
{

    #region Models

    /* Note: only used by OldAccountController
    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordCurrent", NameResourceType = typeof(Resources.ValidationStrings))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(Resources.ValidationStrings))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; }
    }
    */

    /* Note: only used by OldAccountController
    public class AdminChangePasswordModel
    {
        
        public string UserName { get; set; }

        [Required]
        public Guid id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(Resources.ValidationStrings))]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; }
    }
    */

    /* Note: only used by OldAccountController
    public class MembersModel
    {
        //public int personID { get; set; }

        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [DataType(DataType.EmailAddress,
                ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string question { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string answer { get; set; }

        //public bool IsOnline { get; set; }
        //public DateTime CreationDate { get; set; }
        //public DateTime LastActivityDate { get; set; }
        //public DateTime LastLoginDate { get; set; }
        //public Guid ProviderUserKey { get; set; }

        public MembershipUser member { get; set; }
  
        public MembersModel()
        {
        }
        public MembersModel(MembershipUser user)
        {
            this.member = user; 
        }
    }
    */

    /* Note: only used by OldAccountController
    public class LogOnModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Password { get; set; }

        [LocalizedDisplayName("rememberme", NameResourceType = typeof(Resources.ValidationStrings))]
        public bool RememberMe { get; set; }
    }
    */

    /* Note: only used by OldAccountController
    public class RegisterModel
    {
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [DataType(DataType.EmailAddress,
                ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        //[ValidatePasswordLength(ErrorMessageResourceName = "PasswordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [LocalizedDisplayName("password", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; }

        // TODO: Remove - not used
        [StringLength(256)]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string question { get; set; }

        // TODO: Remove - not used
        [StringLength(128)]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string answer { get; set; }
    }
    */

    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    /* Note: only used by OldAccountController
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email, string question, string answer);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
    */

    /* Note: only used by OldAccountController
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }
        //TODO: localize ValidateUser
        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(userName, password);
        }
        //TODO: localize MembershipCreateStatus
        public MembershipCreateStatus CreateUser(string userName, string password, string email, string question, string answer)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, question, answer, true, null, out status);
            return status;
        }
        //TODO: localize ChangePassword
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true); // userIsOnline
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }
    */

    /* Note: only used by OldAccountController
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    */
    #endregion

    #region Validation
    /* Note: only used by OldAccountController
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
    */

    /* Note: only used by OldAccountController
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]{
                new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
            };
        }
    }
    */
    #endregion

}
