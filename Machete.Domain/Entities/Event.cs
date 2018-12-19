#region COPYRIGHT
// File:     Event.cs
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

namespace Machete.Domain
{
    public class Event : Record
    {
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<JoinEventImage> JoinEventImages { get; set; }
        //
        [Required, Column("eventType")]
        public int eventTypeID { get; set; }
        [StringLength(50)]
        public string eventTypeEN { get; set; }
        [StringLength(50)]
        public string eventTypeES { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        [StringLength(4000)]
        public string notes { get; set; }
    }

    public class JoinEventImage : Record
    {
        public int EventID { get; set; }
        public virtual Event Event { get; set; }

        public int ImageID { get; set; }
        public virtual Image Image { get; set; }
    }
}
