using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Activity : Domain.Activity
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
    }

    public class ActivityList
    {
        public int ID { get; set; }
        public string AID { get; set; }         // duplicate names for ids
        public string recordid { get; set; }    // because legacy reasons
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string count { get; set; }
        public string teacher { get; set; }
        public string dateStart { get; set; }
        public string dateEnd { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
    }
}