#region COPYRIGHT
// File:     Activity.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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

namespace Machete.Domain
{
    public class Activity : Record
    {
        public string idChild
        {
            get
            {
                return "asi" + this.ID + "-";
            }
        }
        public virtual ICollection<ActivitySignin> Signins { get; set; }
        //
        [Required, Column("name")]
        public int nameID { get; set; }
        // This is the simplest hack i can come up with to keep search on these two Lookups (previously implement in the app)
        // but push it to the database. 
        [StringLength(50)]
        public string nameEN { get; set; }
        [StringLength(50)]
        public string nameES { get; set; }
        [Required, Column("type")]
        public int typeID { get; set; }
        [StringLength(50)]
        public string typeEN { get; set; }
        [StringLength(50)]
        public string typeES { get; set; }
        [Required]
        public DateTime dateStart { get; set; }
        [Required]
        public DateTime dateEnd { get; set; }
        public bool recurring { get; set; }
        public int firstID { get; set; }
        [Required]
        public string teacher { get; set; }
        [StringLength(4000)]
        public string notes { get; set; }
    }

    public class ActivitySignin: Signin
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }
        public virtual Activity Activity { get; set; }
        public int activityID { get; set; }
        public int? personID { get; set; }
        public virtual Person person {get; set;}
    }
}
