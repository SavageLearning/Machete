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
using Machete.Domain;
using Machete.Web;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel
{
    public class ActivitySchedule : Record
    {
        public IDefaults def;

        public int firstID { get; set; }
                
        [Helpers.LocalizedDisplayName("name", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public int name { get; set; } // lookup
        
        [Helpers.LocalizedDisplayName("type", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "typerequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public int type { get; set; }

        [Helpers.LocalizedDisplayName("dateStart", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateStartrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime dateStart { get; set; }

        [Helpers.LocalizedDisplayName("dateEnd", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateEndrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime dateEnd { get; set; }

        [Helpers.LocalizedDisplayName("teacher", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "teacherrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public string teacher { get; set; }
        
        [Helpers.LocalizedDisplayName("notes", NameResourceType = typeof(Resources.ActivitySchedule))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public string notes { get; set; }

        [Helpers.LocalizedDisplayName("sunday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool sunday { get; set; }
        
        [Helpers.LocalizedDisplayName("monday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool monday { get; set; }
        
        [Helpers.LocalizedDisplayName("tuesday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool tuesday { get; set; }

        [Helpers.LocalizedDisplayName("wednesday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool wednesday { get; set; }

        [Helpers.LocalizedDisplayName("thursday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool thursday { get; set; }
        
        [Helpers.LocalizedDisplayName("friday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool friday { get; set; }

        [Helpers.LocalizedDisplayName("saturday", NameResourceType = typeof(Resources.ActivitySchedule))]
        public bool saturday { get; set; }

        [Helpers.LocalizedDisplayName("stopDate", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateEndrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime stopDate { get; set; }

        public IList<string> teachers { get; set; }
    }
}
