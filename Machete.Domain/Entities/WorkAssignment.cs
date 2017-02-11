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

        //public string fullIDandName()
        //{
        //    if (this.workerAssigned != null) return this.workerAssigned + " " + this.workerAssigned.Person.fullName();
        //    else return null;
        //}

        //public string getFullPseudoID()
        //{
        //    int WONum;
        //    if (this.workOrder == null) WONum = 0;
        //    else if (this.workOrder.paperOrderNum.HasValue) WONum = (int)this.workOrder.paperOrderNum;
        //    else WONum = this.workOrderID;

        //    return System.String.Format("{0,5:D5}", WONum)
        //            + "-" + (this.pseudoID.HasValue ?
        //                System.String.Format("{0,2:D2}", this.pseudoID) :
        //                System.String.Format("{0,5:D5}", this.ID));
        //}
        public void incrPseudoID()
        {
            if (this.workOrder == null) throw new ArgumentNullException("workOrder object is null");            
            this.workOrder.waPseudoIDCounter++;
            this.pseudoID = this.workOrder.waPseudoIDCounter;
        }

        //public double getMinEarnings
        //{
        //    get 
        //    {
        //        return (this.days * this.surcharge) + (this.hourlyWage * this.hours * this.days);
        //    }
        //}
        //public double getMaxEarnings
        //{
        //    get
        //    {
        //        if (this.hourRange == null) return 0;
        //        return (this.days * this.surcharge) + (this.hourlyWage * (int)this.hourRange * this.days);
        //    }
        //}
    }
    public class WorkAssignmentSummary
    {
        public DateTime? date { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }
}