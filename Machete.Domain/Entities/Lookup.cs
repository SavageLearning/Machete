#region COPYRIGHT
// File:     Lookup.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/26 
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    public class Lookup : Record
    {
        public Lookup()
        {
            active = true; // defaulting to true
        }
        //public int ID { get; set; }
        [StringLength(20), Required]
        public string category { get; set; } //Race, Language, M-Status
        [StringLength(50), Required]
        public string text_EN { get; set; }
        [StringLength(50), Required]
        public string text_ES { get; set; }
        public bool selected { get; set; }

        // Skill specific fields
        [StringLength(20)]
        public string subcategory { get; set; } // used in skills; allows hierarchy for skill match
        public int? level { get; set; }      //progression, 0 if unused
        public double? wage { get; set; }
        public int? minHour { get; set; }
        public bool? fixedJob { get; set; }
        public int? sortorder { get; set; }
        public int? typeOfWorkID { get; set; } 
        public bool speciality { get; set; }
        [StringLength(3)]
        public string ltrCode { get; set; }
        [StringLength(4000)]
        [Column(TypeName = "nvarchar(4000)")]
        public string emailTemplate { get; set; }
        [StringLength(300)]
        public string skillDescriptionEn { get; set; }
        [StringLength(300)]
        public string skillDescriptionEs { get; set; }
        public double? minimumCost { get; set; }
        /// <summary>
        /// Set only for records that correspond to internal component or status
        /// </summary>
        [StringLength(30)]
        public string key { get; set; }
        public bool active { get; set; }
    }
}
