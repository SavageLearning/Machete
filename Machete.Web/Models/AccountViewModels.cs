using Machete.Domain;
using Machete.Web.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.Models
{
    public class UserSettingsViewModel
    {
        public string ProviderUserKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string IsApproved { get; set; }
        public string IsLockedOut { get; set; }
        public string IsOnline { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordCurrent", NameResourceType = typeof(ValidationStrings))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordNew", NameResourceType = typeof(ValidationStrings))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string Password { get; set; }

        [LocalizedDisplayName("rememberme", NameResourceType = typeof(ValidationStrings))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[LocalizedDisplayName("username", NameResourceType = typeof(ValidationStrings))]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("firstname", NameResourceType = typeof(ValidationStrings))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("lastname", NameResourceType = typeof(ValidationStrings))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress)]
        [LocalizedDisplayName("EmailAddress", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [ValidatePasswordLength(ErrorMessageResourceName = "PasswordMinLength", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("password", NameResourceType = typeof(ValidationStrings))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("PasswordConfirm", NameResourceType = typeof(ValidationStrings))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordsMustMatch", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }

        [StringLength(256)]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string question { get; set; }

        [StringLength(128)]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string answer { get; set; }
    }
}
