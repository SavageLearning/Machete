using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkAssignment : Domain.WorkAssignment
    {
        public IDefaults def { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }
    }

    public class WorkAssignmentList
    {
        public string tabref { get; set; }          // SharedWAI
        public string tablabel { get; set; }        // SharedWAI
        public int WOID { get; set; }            // SharedWAI, Dispatch
        public int WAID { get; set; }            // Dispatch
        public string recordid { get; set; }
        public string WID { get; set; }             // WorkerWAI
        public string pWAID { get; set; }           // SharedWAI
        public string employername { get; set; }    // WorkerWAI, Dispatch
        public string englishlevel { get; set; }    // SharedWAI, Dispatch
        public string skill { get; set; }           // SharedWAI, WorkerWAI, Dispatch
        public string hourlywage { get; set; }      // SharedWAI, WorkerWAI, Dispatch
        public string hours { get; set; }           // SharedWAI, WorkerWAI, Dispatch
        public string hourRange { get; set; }       // SharedWAI
        public string days { get; set; }            // SharedWAI, WorkerWAI, Dispatch
        public string description { get; set; }     // SharedWAI, WorkerWAI, Dispatch
        public string dateupdated { get; set; }     // SharedWAI
        public string updatedby { get; set; }       // SharedWAI
        public string dateTimeofWork { get; set; }  // WorkerWAI
        public string earnings { get; set; }        // WorkerWAI, Dispatch
        public string assignedWorker { get; set; }  // Dispatch
        public string timeofwork { get; set; }      // Dispatch
        public string asmtStatus { get; set; }      // SharedWAI, WorkerWAI, Dispatch



    }
}