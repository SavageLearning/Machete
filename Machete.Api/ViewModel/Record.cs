using System;

namespace Machete.Api.ViewModel
{
    public class RecordVM : ICloneable
    {
        public string createdby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public int id { get; set; }
        public string updatedby { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}