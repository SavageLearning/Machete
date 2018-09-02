using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkAssignment : Record
    {
        public IDefaults def { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }

        public WorkAssignment()
        {
            idString = "WA";
        }
        //public int ID { get; set; }        
        public int? workerAssignedID { get; set; }
        public virtual Worker workerAssigned { get; set; }

        public int workOrderID { get; set; }
        public virtual WorkOrder workOrder { get; set; }

        public int? workerSigninID { get; set; }
        public virtual WorkerSignin workerSiginin { get; set; }

        public bool active { get; set; }
        // This is relative to the work order...WA1, WA2, WA3...
        [LocalizedDisplayName("pseudoID", NameResourceType = typeof(Resources.WorkOrder))]
        public int? pseudoID { get; set; }

        [LocalizedDisplayName("description", NameResourceType = typeof(Resources.WorkAssignment))]
        [StringLength(1000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        public string description { get; set; }
        //
        [Required(ErrorMessageResourceName = "englishLevelID", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        [LocalizedDisplayName("englishLevelID", NameResourceType = typeof(Resources.WorkAssignment))]
        public int englishLevelID { get; set; }
        //
        [LocalizedDisplayName("skillID", NameResourceType = typeof(Resources.WorkAssignment))]
        [Required(ErrorMessageResourceName = "skillIDRequired", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        public int skillID { get; set; }
        public string skillEN { get; set; }
        public string skillES { get; set; }
        //
        [LocalizedDisplayName("surcharge", NameResourceType = typeof(Resources.WorkAssignment))]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true)]
        public double surcharge { get; set; }
        //
        [LocalizedDisplayName("hourlyWage", NameResourceType = typeof(Resources.WorkAssignment))]
        [Required(ErrorMessageResourceName = "hourlyWagerequired", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true)]
        public double hourlyWage { get; set; }
        //
        [LocalizedDisplayName("hours", NameResourceType = typeof(Resources.WorkAssignment))]
        public double hours { get; set; }
        //
        [LocalizedDisplayName("hourRange", NameResourceType = typeof(Resources.WorkAssignment))]
        public int? hourRange { get; set; }
        //
        [LocalizedDisplayName("days", NameResourceType = typeof(Resources.WorkAssignment))]
        [Required(ErrorMessageResourceName = "daysrequired", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        public int days { get; set; }
        //
        //Evaluation
        [LocalizedDisplayName("qualityOfWork", NameResourceType = typeof(Resources.WorkAssignment))]
        public int qualityOfWork { get; set; }

        [LocalizedDisplayName("followDirections", NameResourceType = typeof(Resources.WorkAssignment))]
        public int followDirections { get; set; }

        [LocalizedDisplayName("attitude", NameResourceType = typeof(Resources.WorkAssignment))]
        public int attitude { get; set; }

        [LocalizedDisplayName("reliability", NameResourceType = typeof(Resources.WorkAssignment))]
        public int reliability { get; set; }

        [LocalizedDisplayName("transportProgram", NameResourceType = typeof(Resources.WorkAssignment))]
        public int transportProgram { get; set; }

        [LocalizedDisplayName("comments", NameResourceType = typeof(Resources.WorkAssignment))]
        public string comments { get; set; }

        [LocalizedDisplayName("workerRating", NameResourceType = typeof(Resources.WorkAssignment))]
        public int? workerRating { get; set; }

        [LocalizedDisplayName("workerRatingComments", NameResourceType = typeof(Resources.Worker))]
        [StringLength(500, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string workerRatingComments { get; set; }


        [LocalizedDisplayName("weightLifted", NameResourceType = typeof(Resources.WorkAssignment))]
        public bool? weightLifted { get; set; }

        public string fullWAID { get; set; }

        public double minEarnings { get; set; }
        public double maxEarnings { get; set; }
        public double? transportCost { get; set; }


    }

    public class WorkAssignmentsList
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
        public string[] requestedList { get; set; } // Dispatch 



    }
}