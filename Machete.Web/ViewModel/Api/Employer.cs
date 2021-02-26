using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel.Api
{
    public class EmployerVM :RecordVM
    {
        public bool active { get; set; }
        [StringLength(50), Required]
        public string address1 { get; set; }
        [StringLength(50)]
        public string address2 { get; set; }
        public bool? blogparticipate { get; set; }
        public bool business { get; set; }
        public string businessname { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]        
        public string cellphone { get; set; }
        [StringLength(50), Required]
        public string city { get; set; }
        [StringLength(30)]
        public string driverslicense { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]        
        public string email { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string fax { get; set; }
        public bool? isOnlineProfileComplete { get; set; }
        [StringLength(10)]
        public string licenseplate { get; set; }
        [StringLength(50), Required]
        public string name { get; set; }
        [StringLength(4000)]
        public string notes { get; set; }
        [StringLength(128)]
        public string onlineSigninID { get; set; }
        public bool onlineSource { get; set; }
        [StringLength(12), Required]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string phone { get; set; }
        public bool receiveUpdates { get; set; }
        public int? referredby { get; set; }
        [StringLength(50)]
        public string referredbyOther { get; set; }
        public bool returnCustomer { get; set; }
        [StringLength(2), Required]
        public string state { get; set; }
        [StringLength(10), Required]
        public string zipcode { get; set; }
    }

    public class EmployersList
    {
        public int id { get; set; }
        public bool active { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string zipcode { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public bool onlineSource { get; set; }
    }
}