using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class Employer : Record
    {
        public int ID { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        //
        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Employer))]
        public bool active { get; set; }
        //
        [LocalizedDisplayName("name", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string name { get; set; }
        //
        [LocalizedDisplayName("address1", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "address1required", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string address1 { get; set; }
        //
        [LocalizedDisplayName("address2", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string address2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Resources.Employer))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string phone { get; set; }
        //
        [LocalizedDisplayName("cellphone", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        //[RequiredIfEmpty("phone", ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string cellphone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("email", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string email { get; set; }
        //
        [LocalizedDisplayName("referredby", NameResourceType = typeof(Resources.Employer))]
        public int? referredby { get; set; }
        //
        [LocalizedDisplayName("referredbyOther", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string referredbyOther { get; set; }
    }
}