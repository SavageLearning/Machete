using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkOrder
    {
        public int ID { get; set; }
        public int EmployerID { get; set; }
        public string WorkSiteAddress1 { get; set; }
        public string WorkSiteAddress2 { get; set; }
        public byte TypeOfWorkID { get; set; }
        public DateTime DateTimeofWork { get; set; }
        public bool TimeFlexible { get; set; }
        public double HourlyWage { get; set; }
        public byte Hours { get; set; }
        public byte HoursChambita { get; set; }
        public byte Days { get; set; }
        public bool EnglishRequired { get; set; }
        //TODO: Stringlength
        public string EnglishRequiredNote { get; set; }
        public bool LunchSupplied { get; set; }
        public bool PermanentPlacement { get; set; }
        //TODO:LOOKUP: Transportmethod
        public byte TransportMethodID { get; set; }
        public double TransportFee { get; set; }
        public double TransportFeeExtra {get; set;}
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public Guid Createdby { get; set; }
        public Guid Updatedby { get; set; }
    }
}