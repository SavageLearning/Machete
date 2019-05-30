using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Machete.Data;
using Machete.Data.Identity;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Microsoft.AspNetCore.Identity;

namespace Machete.Web.ViewModel
{
    public class UserSettingsViewModel
    {
        public string ProviderUserKey { get; set; } // Note: not in db - is this used?

        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }

        public string IsApproved { get; set; }

        public string IsLockedOut { get; set; }

        public string IsOnline { get; set; } // Note: not in db - is this used?

        public DateTime CreationDate { get; set; } // Note: not in db - is this used? Db has CreateDate

        public DateTime LastLoginDate { get; set; }
        
        public bool IsHirer { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordCurrent", NameResourceType = typeof(ValidationStrings))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(ValidationStrings))]
        public string NewPassword { get; set; }
       
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }

        public string Action { get; set; }

        public string ReturnUrl { get; set; }
    }


    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string Password { get; set; }

        [LocalizedDisplayName("rememberme", NameResourceType = typeof(ValidationStrings))]
        public bool RememberMe { get; set; }

        public string Action { get; set; }

        public string ReturnUrl { get; set; }
    }

    // View Model used for Machete worker center members to sign up
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("Password", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }
        
        // Return a pre-populated instance of ApplicationUser:
        public MacheteUser GetUser()
        {
            var user = new MacheteUser
            {
                UserName = FirstName.Trim() + "." + LastName.Trim(),
                Email = Email.Trim()
            };
           
            return user;
        }
    }
    // TODO: need to understand this class better & need to restrict it to only non-employer users
    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(MacheteUser user)
        {
            UserName = user.UserName;
            string[] firstLast = user.UserName.Split('.');
            if (firstLast.Length == 2)
            {
                FirstName = firstLast[0];
                LastName = firstLast[1];
            }
            else
            {
                FirstName = user.UserName;
                LastName = "";
            }
            Email = user.Email;
            IsApproved = user.IsApproved;
            IsLockedOut = user.IsLockedOut;
            Id = user.Id;
        }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }

        public string Id { get; }

        public string ErrorMessage { get; set; }
    }


    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel() { }
        
        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(MacheteUser userBeingModified,
            IEnumerable<MacheteRole> allRoles,
            UserManager<MacheteUser> userManager)
        {
            Roles = new List<SelectRoleEditorViewModel>();
            UserName = userBeingModified.UserName;
            string[] firstLast = userBeingModified.UserName.Split('.');
            FirstName = firstLast[0];
            LastName = firstLast.Length > 1 ? firstLast[1] : "";

            UserId = userBeingModified.Id;

            // ReSharper disable once SuggestVarOrType_Elsewhere
            // Add all available roles to the list of EditorViewModels:
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var selectableRole = new SelectRoleEditorViewModel(role);
                var isInRole = userManager.IsInRoleAsync(userBeingModified, selectableRole.RoleName);
                isInRole.Wait(); // I'm sorry
                if (isInRole.Result) selectableRole.Selected = true;
                Roles.Add(selectableRole);
            }
        }

        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; }

        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; }

        public string UserId { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        
        public SelectRoleEditorViewModel(MacheteRole role)
        {
            RoleName = role.Name;
            RoleId = role.Id;
        }

        public bool Selected { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string RoleName { get; set; }
        public string RoleId { get; }
    }
}
