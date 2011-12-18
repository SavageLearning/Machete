using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;



namespace Machete.Domain
{
    public class Person : Record
    {
        //public override int ID { get; set; }
        public virtual Worker Worker { get; set; }
        public Person() { }
        //
        // Used by EF for Optimistic Concurrency
        //[Timestamp]
        //public Byte[] Timestamp { get; set; }
        //
        //
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
        [LocalizedDisplayName("gender", NameResourceType = typeof(Resources.Person))]
        [Required(ErrorMessageResourceName = "gendererror", ErrorMessageResourceType = typeof(Resources.Person))]
        public int gender { get; set; }
        //
        [LocalizedDisplayName("genderother", NameResourceType = typeof(Resources.Person))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Person))]
        public string genderother { get; set; }

        public string fullName()
        {
            var rtnstr = firstname1 + " ";
            if (firstname2 != null) rtnstr = rtnstr + firstname2 + " ";
            rtnstr = rtnstr + lastname1;
            if (lastname2 != null) rtnstr = rtnstr + " " + lastname2;
            return rtnstr;
        }
    }
}
