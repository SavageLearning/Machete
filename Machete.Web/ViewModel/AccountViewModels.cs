using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Web.Resources;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Machete.Web.ViewModel
{
    public class UserSettingsViewModel
    {
        public string ProviderUserKey { get; set; } // Note: not in db - is this used?

        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(Resources.ValidationStrings))]
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
        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordCurrent", NameResourceType = typeof(Resources.ValidationStrings))]
        public string OldPassword { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(Resources.ValidationStrings))]
        public string NewPassword { get; set; } // Note: not in db - how is this used?
       
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        public string Action { get; set; } // Note: not in db - how is this used?

        public string ReturnUrl { get; set; } // Note: not in db - how is this used?
    }


    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [LocalizedDisplayName("password", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Password { get; set; } // Note: not in db - how is this used?

        [LocalizedDisplayName("rememberme", NameResourceType = typeof(Resources.ValidationStrings))]
        public bool RememberMe { get; set; } // Note: not in db - how is this used?

        public string Action { get; set; } // Note: not in db - how is this used?

        public string ReturnUrl { get; set; } // Note: not in db - how is this used?
    }

    // View Model used for Machete worker center members to sign up
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Password { get; set; } // Note: not in db - how is this used?

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [Compare("Password", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Email { get; set; }
        
        // Return a pre-populated instance of ApplicationUser:
        public MacheteUser GetUser()
        {
            var user = new MacheteUser()
            {
                UserName = this.FirstName.Trim() + "." + this.LastName.Trim(),
                Email = this.Email.Trim(),
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
            this.UserName = user.UserName;
            string[] firstLast = user.UserName.Split('.');
            if (firstLast.Length == 2)
            {
                this.FirstName = firstLast[0];
                this.LastName = firstLast[1];
            }
            else
            {
                this.FirstName = user.UserName;
                this.LastName = "";
            }
            this.Email = user.Email;
            this.IsApproved = user.IsApproved;
            this.IsLockedOut = user.IsLockedOut;
            this.Id = user.Id;
        }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string LastName { get; set; } // Note: not in db - how is this used?

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(Resources.ValidationStrings))]
        public string Email { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        [StringLength(100, ErrorMessageResourceName = "stringLengthMax", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 100 char max
        [RegularExpression(@"^.{6,}$", ErrorMessageResourceName = "passwordLengthMin", ErrorMessageResourceType = typeof(Resources.ValidationStrings))] // 6 char min
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(Resources.ValidationStrings))]
        public string NewPassword { get; set; } // Note: not in db - how is this used?

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(Resources.ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Resources.ValidationStrings))]
        public string ConfirmPassword { get; set; } // Note: not in db - how is this used?

        public string Id { get; set; }

        public string ErrorMessage { get; set; } // Note: not in db - how is this used?
    }


    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(MacheteUser user, IDatabaseFactory dbFactory)
            : this()
        {
            this.UserName = user.UserName;
            string[] firstLast = user.UserName.Split('.');
            this.FirstName = firstLast[0];
            if (firstLast.Length > 1)
            {
                this.LastName = firstLast[1];
            }
            else
            {
                this.LastName = "";
            }

            this.UserId = user.Id;

            // Add all available roles to the list of EditorViewModels:
            IDbSet<IdentityRole> allRoles = dbFactory.Get().Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (IdentityUserRole userRole in user.Roles)
            {
                SelectRoleEditorViewModel checkUserRole =
                    this.Roles.Find(r => r.RoleId == userRole.RoleId);
                checkUserRole.Selected = true;
            }
        }

        [LocalizedDisplayName("username", NameResourceType = typeof(Resources.ValidationStrings))]
        public string UserName { get; set; }

        [LocalizedDisplayName("firstname", NameResourceType = typeof(Resources.ValidationStrings))]
        public string FirstName { get; set; }

        [LocalizedDisplayName("lastname", NameResourceType = typeof(Resources.ValidationStrings))]
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
            this.RoleName = role.Name;
            this.RoleId = role.Id;
        }

        public bool Selected { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
}
