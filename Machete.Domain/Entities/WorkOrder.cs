#region COPYRIGHT
// File:     WorkOrder.cs
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Machete.Domain
{
    public class WorkOrder : Record
    {
        public static int iActive {get; set;}
        public static int iPending {get; set;}
        public static int iCompleted {get; set;}
        public static int iCancelled {get; set;}
        public static int iExpired {get; set;}

        //public int ID { get; set; }
        public int EmployerID { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<WorkAssignment> workAssignments { get; set; }
        public virtual ICollection<WorkerRequest> workerRequests { get; set; }

        private ICollection<JoinWorkOrderEmail> WorkOrderEmails { get; } = new List<JoinWorkOrderEmail>();
        [NotMapped] public ICollection<Email> Emails;

        public WorkOrder()
        {
            waPseudoIDCounter = 0;
            Emails = new JoinCollectionFacade<Email,JoinWorkOrderEmail>(
                WorkOrderEmails,
                woe => woe.Email,
                e => new JoinWorkOrderEmail { WorkOrder = this, Email = e }
            );
        }
        
        public Double timeZoneOffset { get; set; }
        // Flag identifying if source of work order was online web form
        public bool onlineSource { get; set; }
        // Reference to historical paper record order number
        [RegularExpression(@"^$|^[\d]{1,5}$")]
        public int? paperOrderNum { get; set; }
        // Counter to track next work assignment number associated with this work order
        public int waPseudoIDCounter { get; set; }
        // Work site contact name - may be different than employer name
        [StringLength(50)]
        [Required]
        public string contactName { get; set; }
        // Work order status
        [Required]
        [Column("status")]
        public int statusID { get; set; }
        [StringLength(50)]
        public string statusEN { get; set; }
        [StringLength(50)]
        public string statusES { get; set; }
        [StringLength(50), Required]
        public string workSiteAddress1 { get; set; }
        [StringLength(50)]
        public string workSiteAddress2 { get; set; }
        [StringLength(50), Required]
        public string city { get; set; }
        [StringLength(2), Required]
        public string state { get; set; }
        [StringLength(10), Required]
        public string zipcode { get; set; }
        [StringLength(12)]
        [Required, RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string phone { get; set; }
        // Work program (e.g. HHH, DWC, etc)
        // TODO: investigate deleting this - it doesn't appear in the WO interface
        //[Required]
        public int typeOfWorkID { get; set; }
        // Flag indicating if english is required for at least one worker
        public bool englishRequired { get; set; }
        // Description of english skills required
        [StringLength(100)]
        public string englishRequiredNote { get; set; }
        // Flag indicating if lunch is supplied
        public bool lunchSupplied { get; set; }
        // Flag indicating if permanent placement is desired
        public bool permanentPlacement { get; set; }
        // Method of transportation for worker to arrive at work site
        [Required]
        public int transportMethodID { get; set; }
        public string transportMethodEN { get; set; }
        public string transportMethodES { get; set; }
        [Required]
        public int transportProviderID { get; set; }
        // Transportation fee charged for worker transportation 
        [Required]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public double transportFee { get; set; }
        // Extra transportation fee charged for worker transportation 
        [Required]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public double transportFeeExtra { get; set; }
        // Transportation fee payment method
        public int? transportTransactType { get; set; }
        // Transportation transaction fee ID associated with payment (e.g. check number)
        [StringLength(50)]
        public string transportTransactID { get; set; }
        // Work order description
        //[Required]
        [StringLength(4000)]
        public string description { get; set; }
        // Date / time of work
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime dateTimeofWork { get; set; }
        // Flag indicating if the time is flexible
        public bool timeFlexible { get; set; }
        [StringLength(1000)]
        public string additionalNotes { get; set; }
        public bool? disclosureAgreement { get; set; }
        // Fee charged by PayPal for online payments
        public double? ppFee { get; set; }
        [StringLength(5000)]
        public string ppResponse { get; set; }
        [StringLength(25)]
        public string ppPaymentToken { get; set; }
        [StringLength(50)]
        public string ppPaymentID { get; set; }
        [StringLength(25)]
        public string ppPayerID { get; set; }
        [StringLength(20)]
        public string ppState { get; set; }
    }

    public class WorkOrderSummary
    {
        public DateTime? date { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }

}