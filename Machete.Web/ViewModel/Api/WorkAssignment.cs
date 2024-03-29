﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel.Api
{
    public class WorkAssignmentVM : RecordVM
    {
        public string idPrefix { get; }
        public string idString { get; set; }

        public bool active { get; set; }
        public int attitude { get; set; }
        public string comments { get; set; }
        public int days { get; set; }

        [StringLength(1000, ErrorMessage = "field must be atleast 6 characters")]
        public string description { get; set; }
        public int englishLevelID { get; set; }
        public int followDirections { get; set; }
        public string fullWAID { get; set; }
        public double hourlyWage { get; set; }
        public int? hourRange { get; set; }
        public double hours { get; set; }
        public double maxEarnings { get; set; }
        public double minEarnings { get; set; }
        public int? pseudoID { get; set; }
        public int qualityOfWork { get; set; }
        public int reliability { get; set; }
        public string skill { get; set; }
        public int skillID { get; set; }
        public double surcharge { get; set; }
        public double transportCost { get; set; }
        //public int transportProgram { get; set; }
        public bool? requiresHeavyLifting { get; set; }
        // public Worker workerAssigned { get; set; }
        public int? workerAssignedID { get; set; }
        public int? workerRating { get; set; }
        public string workerRatingComments { get; set; }
        // public WorkerSignin workerSiginin { get; set; }
        public int? workerSigninID { get; set; }
        // public WorkOrder workOrder { get; set; }
        public int workOrderID { get; set; }
    }
}
