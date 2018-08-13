#region COPYRIGHT
// File:     Person.cs
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
    public class Person : Record
    {       
        public virtual Worker Worker { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public bool active { get; set; }
        [StringLength(50), Required]
        public string firstname1 { get; set; }
        [StringLength(50)]
        public string firstname2 { get; set; }
        [StringLength(50)]
        public string nickname { get; set; }
        [StringLength(50), Required]
        public string lastname1 { get; set; }
        [StringLength(50)]
        public string lastname2 { get; set; }
        [StringLength(50)]
        public string address1 { get; set; }
        [StringLength(50)]
        public string address2 { get; set; }
        [StringLength(25)]
        public string city { get; set; }
        [StringLength(2)]
        public string state { get; set; }
        [StringLength(10)]
        public string zipcode { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string phone { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$")]
        public string cellphone { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string facebook { get; set; }
        public int gender { get; set; }
        [StringLength(20)]
        public string genderother { get; set; }
        public string fullName { get;  set;}
    }
}
