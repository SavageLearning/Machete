#region COPYRIGHT
// File:     viewOptions.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
// License:  GPL v3
// Project:  Machete.Service
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
using System.Globalization;

namespace Machete.Service
{
    public class viewOptions
    {
        public string employerGuid;
        public CultureInfo CI;
        public string category;
        public string sSearch;
        public DateTime? date;
        public int? EmployerID { get; set; }
        public int? dwccardnum;
        public int? woid;
        public int? status;
        public bool? onlineSource;
        public bool showPending;
        public bool orderDescending;
        public int displayStart = 0;
        public int displayLength = 0;
        public string sortColName;
        public string wa_grouping;
        public int? typeofwork_grouping;
        public int? activityID;
        public int personID;
        //public bool showOrdersPending;
        //public bool showOrdersWorkers;
        //public bool showInactiveWorker;
        //public bool showSanctionedWorker;
        //public bool showExpiredWorker;
        public bool showExpiredWorkers;
        public bool showSExWorkers;
        public bool showNotWorkers;
        public bool showWorkers;
        //public bool showExpelledWorker;
        public bool attendedActivities;
        public bool authenticated = true;
        public int? emailID { get; set; }
    }
}
