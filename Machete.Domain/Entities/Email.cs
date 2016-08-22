#region COPYRIGHT
// File:     Email.cs
// Author:   Savage Learning, LLC.
// Created:  2013/05/02
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
    public class Email : Record
    {
        /// <summary>
        /// Integer for this state defined in Lookups
        /// </summary>        
        public static int iPending { get; set; }
        /// <summary>
        /// Integer for this state defined in Lookups
        /// </summary>        
        public static int iReadyToSend { get; set; }
        /// <summary>
        /// Integer for this state defined in Lookups
        /// </summary>        
        public static int iSending { get; set; }
        /// <summary>
        /// Integer for this state defined in Lookups
        /// </summary>        
        public static int iSent { get; set; }
        /// <summary>
        /// Integer for this state defined in Lookups
        /// </summary>
        public static int iTransmitError { get; set; }
        /// <summary>
        /// Limit on the number of attempts to send emails. Read from Config file.
        /// </summary>
        public static int iTransmitAttempts { get; set; }
        public Email()
        {
            statusID = Email.iPending;
            idString = "email";
        }

        public virtual ICollection<WorkOrder> WorkOrders { get; set; }


        [StringLength(50)]
        public string emailFrom { get; set; }
        //
        [StringLength(50),Required()]
        public string emailTo { get; set; }
        //
        [StringLength(100),Required()]
        public string subject { get; set; }
        //
        [StringLength(8000),Required()]
        [Column(TypeName = "nvarchar(MAX)")]
        public string body { get; set; }
        public int transmitAttempts { get; set; }
        public int  statusID { get; set; }
        public DateTime? lastAttempt { get; set; }
        public string attachment { get; set; }
        public string attachmentContentType { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [NotMapped]
        public bool isAssociatedToWorkOrder
        {
            get
            {
                if (this.WorkOrders.Count() > 0) return true;
                return false;
            }
        }
        [NotMapped]
        public IQueryable<WorkOrder> AssociatedWorkOrders
        {
            get
            {
                return this.WorkOrders.AsQueryable();
            }
        }
        [NotMapped]
        public WorkOrder currentAssociatedWorkorder
        {
            get
            {
                return this.AssociatedWorkOrders.OrderByDescending(wo => wo.paperOrderNum).FirstOrDefault();
            }
        }
    }


    public class JoinWorkorderEmail : Record
    {
        public int WorkOrderID { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public int EmailID { get; set; }
        public virtual Email Email { get; set; }

    }
}
