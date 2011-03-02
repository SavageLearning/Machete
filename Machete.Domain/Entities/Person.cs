using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;



namespace Machete.Domain
{

    public class Person
    {
        public int ID { get; set; }
        //
        [LocalizedDisplayName("firstname1", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        [Required(ErrorMessageResourceName = "firstname1error", ErrorMessageResourceType = typeof(Persons))]
        public string firstname1 { get; set; }
        //
        [LocalizedDisplayName("firstname2", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string firstname2 { get; set; }
        //
        [LocalizedDisplayName("lastname1", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        [Required(ErrorMessageResourceName = "lastname1error", ErrorMessageResourceType = typeof(Persons))]
        public string lastname1 { get; set; }
        //
        [LocalizedDisplayName("lastname2", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string lastname2 { get; set; }
        //
        [LocalizedDisplayName("address1", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string address1 { get; set; }
        //
        [LocalizedDisplayName("address2", NameResourceType = typeof(Persons))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string address2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Persons))]
        [StringLength(25, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Persons))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Persons))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Persons))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string phone { get; set; }
        // TODO: GUI drop down & limit gender characters
        [LocalizedDisplayName("gender", NameResourceType = typeof(Persons))]
        [StringLength(1, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        [Required(ErrorMessageResourceName = "gendererror", ErrorMessageResourceType = typeof(Persons))]
        public string gender { get; set; }
        //
        [LocalizedDisplayName("genderother", NameResourceType = typeof(Persons))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Persons))]
        public string genderother { get; set; }

        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public Guid Createdby { get; set; }
        public Guid Updatedby { get; set; }
    }
}
