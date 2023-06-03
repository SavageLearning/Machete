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

    public class ListVM : ICloneable
    {
        public int ID { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}