using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Record : ICloneable
    {
        public string idString { get; set; }
        public int ID { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        [StringLength(30)]
        public string createdby { get; set; }
        [StringLength(30)]
        public string updatedby { get; set; }
        //private static int byLength = 30;
        public Record() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string idPrefix
        {
            get
            {
                return idString + this.ID.ToString() + "-";
            }
        }

    }
}