using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Api.ViewModel
{
    public class PersonVM : RecordVM
    {
        public bool active { get; set; }
        [StringLength(50), Required]
        public string firstname1 { get; set; }
        [StringLength(50)]
        public string firstname2 { get; set; }
        [StringLength(50)]
        public string nickname { get; set; }
        [StringLength(50), Required]
        public string lastname1 { get; set; }
        [StringLength(50)]
        public string lastname2 { get; set; }
        [StringLength(50)]
        public string address1 { get; set; }
        [StringLength(50)]
        public string address2 { get; set; }
        [StringLength(25)]
        public string city { get; set; }
        [StringLength(2)]
        public string state { get; set; }
        [StringLength(10)]
        public string zipcode { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string phone { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string cellphone { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string facebook { get; set; }
        public int gender { get; set; }
        [StringLength(20)]
        public string genderother { get; set; }
        public string fullName { get; set; }
    }

    public class PersonListVM : ListVM
    {

    }
}