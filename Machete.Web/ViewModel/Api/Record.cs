using System;

namespace Machete.Web.ViewModel.Api
{
    public class RecordVM : ICloneable
    {
        public string createdby { get; set; }
        public string datecreated { get; set; }
        public string dateupdated { get; set; }
        public int id { get; set; }
        public string updatedby { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}