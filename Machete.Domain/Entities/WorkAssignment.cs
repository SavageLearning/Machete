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
        public int? workerAssignedID { get; set; }
        public virtual Worker workerAssigned { get; set; }

        public int workOrderID { get; set; }
        public virtual WorkOrder workOrder { get; set; }

        public int? workerSigninID { get; set; }
        public virtual WorkerSignin workerSiginin { get; set; }

        public bool active { get; set; }
        // This is relative to the work order...WA1, WA2, WA3...
        public int? pseudoID { get; set; }

        [StringLength(1000)]
        public string description { get; set; }
        [Required]
        public int englishLevelID { get; set; }
        [Required]
        public int skillID { get; set; }       
        public string skillEN { get; set; }
        public string skillES { get; set; }
        //
        public double surcharge { get; set; }
        //
        [Required]
        public double hourlyWage { get; set; }
        public double hours { get; set; }
        public int? hourRange { get; set; }
        [Required]
        public int days { get; set; }
        //
        //Evaluation
        public int qualityOfWork { get; set; }
        public int followDirections { get; set; }
        public int attitude { get; set; }
        public int reliability { get; set; }
        public int transportProgram { get; set; }
        public string comments { get; set; }
        public int? workerRating { get; set; }
        [StringLength(500)]
        public string workerRatingComments { get; set; }

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