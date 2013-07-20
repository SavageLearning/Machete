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
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class ActivitySchedule : Record
    {
        public ActivitySchedule()
        {
            idString = "activity";
        }

        public string idChild
        {
            get
            {
                return "asi" + this.ID + "-";
            }
        }
        public virtual ICollection<ActivityScheduleSignin> Signins { get; set; }
        //
        [LocalizedDisplayName("name", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public int name { get; set; } // lookup
        //
        [LocalizedDisplayName("type", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "typerequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public int type { get; set; }
        // This is the date and time the event will start in the standard Activity scheduler
        [LocalizedDisplayName("dateStart", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateStartrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime dateStart { get; set; }
        // This is the date and time the event will end
        [LocalizedDisplayName("dateEnd", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateEndrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime dateEnd { get; set; }
        //
        [LocalizedDisplayName("teacher", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "teacherrequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public string teacher { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.ActivitySchedule))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public string notes { get; set; }
        // This determines whether the event is a daily recurring event
        [LocalizedDisplayName("daily", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool daily { get; set; }
        // This determines whether the event is a weekly recurring event
        [LocalizedDisplayName("weekly", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool weekly { get; set; }
        // Event happens Mondays
        [LocalizedDisplayName("monday", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool monday { get; set; }
        // Event happens Tuesdays
        [LocalizedDisplayName("tuesday", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool tuesday { get; set; }
        // Event happens Wednesdays
        [LocalizedDisplayName("wednesday", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool wednesday { get; set; }
        // Event happens Thursdays
        [LocalizedDisplayName("thursday", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool thursday { get; set; }
        // Event happens Fridays
        [LocalizedDisplayName("friday", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool friday { get; set; }
        // This is the field that determines on what date (and time) the recurring event will begin automating itself
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.ActivitySchedule))]
        // [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public bool saturday { get; set; }
        // This is the field that determines on what date (and time) the recurring event will stop automating itself
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateStartRequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime beginOn { get; set; }
        //
        [LocalizedDisplayName("endOn", NameResourceType = typeof(Resources.ActivitySchedule))]
        [Required(ErrorMessageResourceName = "dateEndRequired", ErrorMessageResourceType = typeof(Resources.ActivitySchedule))]
        public DateTime endOn { get; set; }
        //
    }

    public class ActivityScheduleSignin: Signin
    {
        public ActivityScheduleSignin()
        {
            idString = "asi";
        }
        public virtual Activity Activity { get; set; }
        public int activityID { get; set; }
        public int? personID { get; set; }
        public virtual Person person {get; set;}
    }
}
