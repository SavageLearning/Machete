#region COPYRIGHT
// File:     WorkerSignin.cs
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
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    public class WorkerSignin : Signin 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }
        public int? WorkAssignmentID { get; set; }
        public DateTime? lottery_timestamp { get; set; }
        public int? lottery_sequence { get; set; }
        public virtual Worker worker { get; set; }
        public int? WorkerID { get; set; }

    }
    public abstract class Signin : Record
    {
        [Required, RegularExpression("^[0-9]{5,5}$")]
        public virtual int dwccardnum { get; set; } 
        [Column("memberStatus")]
        public int? memberStatusID { get; set; }
        public DateTime dateforsignin { get; set; }
        public Double timeZoneOffset { get; set; }
    }
}
