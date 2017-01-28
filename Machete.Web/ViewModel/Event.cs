using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Event : Domain.Event
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
    }

    public class EventList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string fileCount { get; set; }
        public string type { get; set; }
        public string dateupdated { get; set; }
        public string notes { get; set; }
        public string updatedby { get; set; }
    }

}