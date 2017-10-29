#region COPYRIGHT
// File:     WorkAssignment.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Domain
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class WorkAssignment : Record
    {        
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
    public class WorkAssignmentSummary
    {
        public DateTime? date { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }
}