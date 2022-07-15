using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Machete.Domain;

namespace Machete.Service.CustomValidators
{
    public class UserEditableConfigAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"This record is not user-editable";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var key = (String)value;
            if (!UserDefinedConfigs.Lower.Contains(key.ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

    }
}
