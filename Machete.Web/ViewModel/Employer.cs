using Machete.Web.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel
{
    public class Employer : Record
    {
        public Employer()
        {
            idString = "employer";
        }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Employer))]
        public bool active { get; set; }
        [LocalizedDisplayName("onlineSource", NameResourceType = typeof(Resources.Employer))]
        public string onlineSource { get; set; }
        [LocalizedDisplayName("isbusiness", NameResourceType = typeof(Resources.Employer))]
        public bool business { get; set; }
        public string isOnlineProfileComplete { get; set; }
        [LocalizedDisplayName("receiveUpdates", NameResourceType = typeof(Resources.Employer))]
        public string receiveUpdates { get; set; }
        [LocalizedDisplayName("returnCustomer", NameResourceType = typeof(Resources.Employer))]
        public string returnCustomer { get; set; }
        [LocalizedDisplayName("businessname", NameResourceType = typeof(Resources.Employer))]
        public string businessname { get; set; }


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
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string phone { get; set; }
        //
        [LocalizedDisplayName("fax", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string fax { get; set; }

        [LocalizedDisplayName("cellphone", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string cellphone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("email", NameResourceType = typeof(Resources.Employer))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string email { get; set; }
        //
        [LocalizedDisplayName("licenseplate", NameResourceType = typeof(Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string licenseplate { get; set; }
        //
        [LocalizedDisplayName("driverslicense", NameResourceType = typeof(Resources.Employer))]
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string driverslicense { get; set; }
        //
        [LocalizedDisplayName("referredby", NameResourceType = typeof(Resources.Employer))]
        public int? referredby { get; set; }
        //
        [LocalizedDisplayName("referredbyOther", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string referredbyOther { get; set; }
        //
        [LocalizedDisplayName("blogparticipate", NameResourceType = typeof(Resources.Employer))]
        public bool? blogparticipate { get; set; }

        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Employer))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string notes { get; set; }

        [StringLength(128, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string onlineSigninID { get; set; }


    }

    public class EmployerList 
    {
        public int ID { get; set; }
        public string EID { get; set; }         // duplicate names for ids
        public string recordid { get; set; }    // because legacy reasons
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string active { get; set; }
        public string dateupdated { get; set; }
        public string onlineSource { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string zipcode { get; set; }
        public string updatedby { get; set; }
    }
}