#region COPYRIGHT
// File:     WorkAssignmentIndex.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
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
using Machete.Web.Helpers;

namespace Machete.Web.ViewModel
{
    /// <summary>
    /// Class for /WorkAssignment/Index view. 
    /// </summary>
    public class WorkAssignmentIndex
    {
        public string todaysdate { get; set; }
        public string dwccardnum { get; set; }
        public int status { get; set; }
        public bool wa_grouping { get; set; }
        public int typeofwork_grouping { get; set; }
        public bool assignedWorker_visible { get; set; }
        public bool signin_visible { get; set; }
        public bool requestedWorkers_visible { get; set; }
        public WorkerSignin _wsi { get; set; }
        public IDefaults def;
    }
}