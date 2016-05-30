#region COPYRIGHT
// File:     Employer.cs
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Employer : Record
    {
        public Employer()
        {
            idString = "employer";
        }
        //public int ID { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        //
        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Employer))]
        public bool active { get; set; }
        //
        [LocalizedDisplayName("onlineSource", NameResourceType = typeof(Resources.Employer))]
        public bool onlineSource { get; set; }
        //
        [LocalizedDisplayName("returnCustomer", NameResourceType = typeof(Resources.Employer))]
        public bool returnCustomer { get; set; }
        //
        [LocalizedDisplayName("receiveUpdates", NameResourceType = typeof(Resources.Employer))]
        public bool receiveUpdates { get; set; }
        //
        [LocalizedDisplayName("isbusiness", NameResourceType = typeof(Resources.Employer))]
        public bool business { get; set; }

        [LocalizedDisplayName("businessname", NameResourceType = typeof(Resources.Employer))]
        public string businessname { get; set;}

        [LocalizedDisplayName("name", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string name { get; set; }
        //
        [LocalizedDisplayName("address1", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "address1required", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string address1 { get; set; }
        //
        [LocalizedDisplayName("address2", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string address2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Resources.Employer))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string phone { get; set; }
        //
        [LocalizedDisplayName("fax", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string fax { get; set; }

        [LocalizedDisplayName("cellphone", NameResourceType = typeof(Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        //[RequiredIfEmpty("phone", ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.Employer))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string cellphone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("email", NameResourceType = typeof(Resources.Employer))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "emailValidation", ErrorMessageResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string email { get; set; }
        //
        [LocalizedDisplayName("licenseplate", NameResourceType = typeof(Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string licenseplate { get; set; }
        //
        [LocalizedDisplayName("driverslicense", NameResourceType = typeof(Resources.Employer))]
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string driverslicense { get; set; }
        //
        [LocalizedDisplayName("referredby", NameResourceType = typeof(Resources.Employer))]
        public int? referredby { get; set; }
        //
        [LocalizedDisplayName("referredbyOther", NameResourceType = typeof(Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string referredbyOther { get; set; }
        //
        [LocalizedDisplayName("blogparticipate", NameResourceType = typeof(Resources.Employer))]
        public bool? blogparticipate { get; set; }

        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Employer))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string notes { get; set; }

        [StringLength(128, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Employer))]
        public string onlineSigninID { get; set; }

        public bool? isOnlineProfileComplete { get; set; }
    }
}