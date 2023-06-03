using System;
using System.ComponentModel.DataAnnotations;
using Machete.Api.Helpers;

namespace Machete.Api.ViewModel
{
    public abstract class SigninVM : RecordVM
    {
        [Required]
        [RegularExpression("^[0-9]{5,5}$")]
        public virtual int dwccardnum { get; set; }
        public int? memberStatusID { get; set; }
        public DateTime dateforsignin { get; set; }
    }
}