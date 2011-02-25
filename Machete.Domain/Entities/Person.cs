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
        //TODO: 1: Finish restrictions on Person
        public int ID { get; set; }
        [LocalizedDisplayName("firstname1", NameResourceType = typeof(Persons))]
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "firstname1error", ErrorMessageResourceType = typeof(Persons))]
        public string firstname1 { get; set; }
        [LocalizedDisplayName("firstname2", NameResourceType = typeof(Persons))]
        [StringLength(50)]
        public string firstname2 { get; set; }
        [LocalizedDisplayName("lastname1", NameResourceType = typeof(Persons))]
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "lastname1error", ErrorMessageResourceType = typeof(Persons))]
        public string lastname1 { get; set; }
        [LocalizedDisplayName("lastname2", NameResourceType = typeof(Persons))]
        [StringLength(50)]
        public string lastname2 { get; set; }
        [LocalizedDisplayName("address1", NameResourceType = typeof(Persons))]
        public string address1 { get; set; }
        [LocalizedDisplayName("address2", NameResourceType = typeof(Persons))]
        public string address2 { get; set; }
        [LocalizedDisplayName("city", NameResourceType = typeof(Persons))]
        public string city { get; set; }
        [LocalizedDisplayName("state", NameResourceType = typeof(Persons))]
        public string state { get; set; }
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Persons))]
        public string zipcode { get; set; }
        [LocalizedDisplayName("phone", NameResourceType = typeof(Persons))]
        public string phone { get; set; }
        [LocalizedDisplayName("gender", NameResourceType = typeof(Persons))]
        public string gender { get; set; }
        [LocalizedDisplayName("genderother", NameResourceType = typeof(Persons))]
        public string genderother { get; set; }
        // TODO: Have PersonController update time settings
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }
}
