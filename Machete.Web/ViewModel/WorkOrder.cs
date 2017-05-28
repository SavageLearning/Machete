using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkOrder : Domain.WorkOrder
    {
        public IDefaults def { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }
        public string recordid { get; set; }
        public string dateupdatedstring { get; set; }
        public string datecreatedstring { get; set; }
        public string transportMethod { get; set; }
    }

    public class WorkOrdersList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }
        public string dateTimeofWork { get; set; }
        public string status { get; set; }
        public int statusID { get; set; }
        public string displayState { get; set; }
        public string transportMethod { get; set; }
        public int WAcount { get; set; }
        public string contactName { get; set; }
        public string workSiteAddress1 { get; set; }
        public string zipcode { get; set; }
        public string onlineSource { get; set; }
        public string emailSentCount { get; set; }
        public string emailErrorCount { get; set; }
        public string updatedby { get; set; }
        public string dateupdated { get; set; }
        public string recordid { get; set; }
        public IEnumerable<WorkerAssignedList> workers { get; set; }
    }

    public class WorkerAssignedList
    {
        public int WID { get; set; }
        public string name { get; set; }
        public string skill { get; set; }
        public double hours { get; set; }
        public double wage { get; set; }
    }

    //workers = showWorkers? // Note: Workers appears to not be used
    //    from w in wo.workAssignments
    //    select new
    //                    {
    //                        WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
    //                        name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
    //                        skill = lcache.textByID(w.skillID, CI.TwoLetterISOLanguageName),
    //                        hours = w.hours,
    //                        wage = w.hourlyWage
    //} : null
}