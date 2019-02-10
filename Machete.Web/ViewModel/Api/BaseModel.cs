using System;

namespace Machete.Web.ViewModel.Api
{
    public class BaseModel
    {
        public int id { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }
    }
}