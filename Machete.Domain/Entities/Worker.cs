using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class Worker
    {
        public int ID { get; set; }
        public byte raceID { get; set; }
        // Error from nullstring:: The type 'string' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'System.Nullable<T>'
        [StringLength(20)]
        //TODO: localize worker **32** fields
        public string raceother { get; set; }
        [StringLength(10)]
        public string height {get; set;}
        [StringLength(10)]
        public string weight {get; set;}
        public byte englishlevelID {get; set;}
        public bool recentarrival {get; set;}
        public DateTime dateinUSA { get; set;}
        public DateTime dateinseattle {get; set;}
        public bool disabled {get; set;}
        [StringLength(50)]
        public string disabilitydesc {get; set;}
        public char maritalstatus {get; set;}
        public bool livewithchildren {get; set;}
        //TODO: positive number check
        public byte numofchildren {get; set;}
        public byte incomeID {get; set;}
        public bool livealone {get; set;}
        [StringLength(50)]
        public string emcontUSAname {get; set;}
        [StringLength(30)]
        public string emcontUSArelation {get; set;}
        [StringLength(14)]
        public string emcontUSAphone {get; set;}
        public int dwccardnum {get; set;}
        public byte seattleneighborhoodID {get; set;}
        public bool immigrantrefugee {get; set;}
        [StringLength(20)]
        public string countryoforigin {get; set;}
        [StringLength(50)]
        public string emcontoriginname {get; set;}
        [StringLength(30)]
        public string emcontoriginrelation {get; set;}
        [StringLength(14)]
        public string emcontoriginphone {get; set;}
        //TODO: how to handle imaage from SQL?
        public DateTime memberexpirationdate {get; set;}
        public bool driverslicense {get; set;}
        //TODO: If driverslicense is true, then expiration date check
        public DateTime? expirationdate {get; set;}
        public bool? insurance {get; set;}
        public DateTime? insuranceexpiration { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }
}

