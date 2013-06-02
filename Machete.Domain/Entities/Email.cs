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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    public class Email : Record
    {
        public static int iPending { get; set; }
        public static int iReadyToSend { get; set; }
        public static int iSending { get; set; }
        public static int iSent { get; set; }
        public static int iTransmitError { get; set; }
        public Email()
        { 
            idString = "email";
        }

        [LocalizedDisplayName("emailFrom", NameResourceType = typeof(Resources.Email))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Email))]
        public string emailFrom { get; set; }
        //
        [LocalizedDisplayName("emailTo", NameResourceType = typeof(Resources.Email))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Email))]
        [Required(ErrorMessageResourceName = "emailTo", ErrorMessageResourceType = typeof(Resources.Email))]
        public string emailTo { get; set; }
        //
        [LocalizedDisplayName("subject", NameResourceType = typeof(Resources.Email))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Email))]
        [Required(ErrorMessageResourceName = "subject", ErrorMessageResourceType = typeof(Resources.Email))]
        public string subject { get; set; }
        //
        [LocalizedDisplayName("body", NameResourceType = typeof(Resources.Email))]
        [StringLength(8000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Email))]
        [Required(ErrorMessageResourceName = "body", ErrorMessageResourceType = typeof(Resources.Email))]
		[Column(TypeName = "nvarchar(MAX)")]
        public string body { get; set; }

        public int transmitAttempts { get; set; }
        public int  status { get; set; }
        public DateTime? lastAttempt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }


    public class JoinWorkorderEmail : Record
    {
        public int WorkOrderID { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public int EmailID { get; set; }
        public virtual Email Email { get; set; }

    }
}
