using System;

namespace Machete.Service.DTO
{
    public class WorkforceList
    {
        public int id { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public int dwccardnum { get; set; }
        public DateTime memberexpirationdate { get; set; }
        public string memberStatusEN { get; set; }
        public bool? driverslicense { get; set; }
        public bool? carinsurance { get; set; }
        public DateTime? insuranceexpiration { get; set; }
        public DateTime dateupdated { get; set; }
        public int memberStatusID { get; set; }
        public string skillCodes { get; set; } 
        public string zipCode { get; set; }
    }
}
