using Machete.Web.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel
{
    public class Person : Record
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

        public Person()
        {
            idString = "person";
        }
        public virtual Worker Worker { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Person))]
        public bool active { get; set; }
        //
        [LocalizedDisplayName("firstname1", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        [Required(ErrorMessageResourceName = "firstname1error", ErrorMessageResourceType = typeof(Resources.Person))]
        public string firstname1 { get; set; }
        //
        [LocalizedDisplayName("firstname2", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string firstname2 { get; set; }
        //
        [LocalizedDisplayName("nickname", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string nickname { get; set; }

        [LocalizedDisplayName("lastname1", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        [Required(ErrorMessageResourceName = "lastname1error", ErrorMessageResourceType = typeof(Resources.Person))]
        public string lastname1 { get; set; }
        //
        [LocalizedDisplayName("lastname2", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string lastname2 { get; set; }
        //
        [LocalizedDisplayName("address1", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string address1 { get; set; }
        //
        [LocalizedDisplayName("address2", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string address2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Resources.Person))]
        [StringLength(25, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Resources.Person))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.Person))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Resources.Person))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Person))]
        public string phone { get; set; }

        [LocalizedDisplayName("cellphone", NameResourceType = typeof(Resources.Person))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Person))]
        public string cellphone { get; set; }

        [LocalizedDisplayName("email", NameResourceType = typeof(Resources.Person))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string email { get; set; }

        [LocalizedDisplayName("facebook", NameResourceType = typeof(Resources.Person))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string facebook { get; set; }

        [LocalizedDisplayName("gender", NameResourceType = typeof(Resources.Person))]
        //[Required(ErrorMessageResourceName = "gendererror", ErrorMessageResourceType = typeof(Resources.Person))]
        public int gender { get; set; }

        [LocalizedDisplayName("genderother", NameResourceType = typeof(Resources.Person))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string genderother { get; set; }

        public string fullName { get; set; }
    }

    public class PersonList
    {
        public int ID { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string dwccardnum { get; set; }
        public string active { get; set; }
        public string status { get; set; }
        public string workerStatus { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string phone { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
        public string recordid { get; set; }

    }
}