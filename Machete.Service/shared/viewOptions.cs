using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Machete.Service
{
    public class viewOptions
    {
        public CultureInfo CI;
        public string category;
        public string sSearch;
        public DateTime? date;
        public int? EmployerID { get; set; }
        public int? dwccardnum;
        public int? woid;
        public int? status;
        public bool showPending;
        public bool orderDescending;
        public int displayStart = 0;
        public int displayLength = 0;
        public string sortColName;
        public string wa_grouping;
        public int? typeofwork_grouping;
        public int? activityID;
        public int personID;
        //public bool showOrdersPending;
        //public bool showOrdersWorkers;
        //public bool showInactiveWorker;
        //public bool showSanctionedWorker;
        //public bool showExpiredWorker;
        //public bool showExpelledWorker;
        public bool attendedActivities;
        public bool authenticated = true;
    }
}
