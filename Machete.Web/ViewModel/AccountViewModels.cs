using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordCurrent", NameResourceType = typeof(ValidationStrings))]
        public string OldPassword { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(ValidationStrings))]
        public string NewPassword { get; set; } // Note: not in db - how is this used?
       
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        public string Action { get; set; } // Note: not in db - how is this used?

        public string ReturnUrl { get; set; } // Note: not in db - how is this used?
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
        public string Password { get; set; } // Note: not in db - how is this used?

        [LocalizedDisplayName("rememberme", NameResourceType = typeof(ValidationStrings))]
        public bool RememberMe { get; set; } // Note: not in db - how is this used?

        public string Action { get; set; } // Note: not in db - how is this used?

        public string ReturnUrl { get; set; } // Note: not in db - how is this used?
    }

    // View Model used for Machete worker center members to sign up
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string Password { get; set; } // Note: not in db - how is this used?

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("Password", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?

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
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?

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
        public string NewPassword { get; set; } // Note: not in db - how is this used?

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        public string Id { get; set; }

        public string ErrorMessage { get; set; } // Note: not in db - how is this used?
    }


    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            Roles = new List<SelectRoleEditorViewModel>();
        }

        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(MacheteUser user, MacheteContext dbFactory)
            : this()
        {
            UserName = user.UserName;
            string[] firstLast = user.UserName.Split('.');
            FirstName = firstLast[0];
            if (firstLast.Length > 1)
            {
                LastName = firstLast[1];
            }
            else
            {
                LastName = "";
            }

            UserId = user.Id;

            // ReSharper disable once SuggestVarOrType_Elsewhere
            // Add all available roles to the list of EditorViewModels:
            DbSet<IdentityRole> allRoles = dbFactory.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                SelectRoleEditorViewModel checkUserRole =
                    Roles.Find(r => r.RoleId == userRole.Id);
                checkUserRole.Selected = true;
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
        public SelectRoleEditorViewModel() {}
        public SelectRoleEditorViewModel(IdentityRole role)
        {
            RoleName = role.Name;
            RoleId = role.Id;
        }

        public bool Selected { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
}
