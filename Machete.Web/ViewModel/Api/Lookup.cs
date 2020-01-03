using System;

namespace Machete.Web.ViewModel.Api
{
    public class LookupVM
    {
        public string createdby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public int id { get; set; }
        public string idPrefix { get; }
        public string idString { get; set; }
        public string updatedby { get; set; }

        public bool active { get; set; }
        public string category { get; set; }
        public string emailTemplate { get; set; }
        public bool? fixedJob { get; set; }
        public string key { get; set; }
        public int? level { get; set; }
        public string ltrCode { get; set; }
        public int? minHour { get; set; }
        public double? minimumCost { get; set; }
        public bool selected { get; set; }
        public string skillDescriptionEn { get; set; }
        public string skillDescriptionEs { get; set; }
        public int? sortorder { get; set; }
        public bool speciality { get; set; }
        public string subcategory { get; set; }
        public string text_EN { get; set; }
        public string text_ES { get; set; }
        public int? typeOfWorkID { get; set; }
        public double? wage { get; set; }
        public string clientRules { get; set; }
    }
}