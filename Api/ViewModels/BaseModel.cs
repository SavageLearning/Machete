using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
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