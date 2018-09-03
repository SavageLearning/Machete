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
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        //
        public bool active { get; set; }
        public bool onlineSource { get; set; }
        public bool returnCustomer { get; set; }
        public bool receiveUpdates { get; set; }
        public bool business { get; set; }
        public string businessname { get; set;}

        [StringLength(50), Required]
        public string name { get; set; }
        [StringLength(50), Required]
        public string address1 { get; set; }
        [StringLength(50)]
        public string address2 { get; set; }
        [StringLength(50), Required]
        public string city { get; set; }
        [StringLength(2), Required]
        public string state { get; set; }
        [StringLength(12), Required]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string phone { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string fax { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string cellphone { get; set; }
        [StringLength(10), Required]
        public string zipcode { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(10)]
        public string licenseplate { get; set; }
        [StringLength(30)]
        public string driverslicense { get; set; }
        public int? referredby { get; set; }
        [StringLength(50)]
        public string referredbyOther { get; set; }
        public bool? blogparticipate { get; set; }
        [StringLength(4000)]
        public string notes { get; set; }
        [StringLength(128)]
        public string onlineSigninID { get; set; }
        public bool? isOnlineProfileComplete { get; set; }
    }
}