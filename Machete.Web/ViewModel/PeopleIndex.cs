#region COPYRIGHT
// File:     PeopleIndex.cs
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

namespace Machete.Web.ViewModel
{
    /// <summary>
    /// Class for /Persons/Index view. 
    /// </summary>
    public class PeopleIndex
    {
        public bool showWorkers { get; set; }
        public bool showNotWorkers { get; set; }
        public bool showExpiredWorkers { get; set; }
        public bool showSExWorkers { get; set; }
    }

    /// <summary>
    /// Class for Workers/Persons combined datatable view
    /// </summary>
    public class PersonsView : Person
    {
        public string dwccardnum { get; set; }
        public int workerStatus { get; set; }
    }
}