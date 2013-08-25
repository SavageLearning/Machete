#region COPYRIGHT
// File:     EmployerWoCombined.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using System.ComponentModel.DataAnnotations;


namespace Machete.Web.ViewModel
{
    public class monthlyWithDetail
    {   
        public mwdViewData displayViewData { get; set; }
    }

    //was mostly using this one to mess around with the MVC layer
    //and see if I could get it working...
    //might be more worth it to create an actual array
    //but that would double the work
    // yes, referring to previous comment, why does this exist? not sure this is necessary....
    public class mwdViewData
    {
        /// A class containing all of the data for the Monthly Report with Details
        /// DateTime date, int totalDWCSignins, int totalHHHSignins
        /// dispatchedDWCSignins, int dispatchedHHHSignins
        /// int totalHours, double totalIncome, ...
        /// double totalAverage (commented out, not working)
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        public int TotalSignins { get; set; }
        public int totalDWCSignins { get; set; }
        public int totalHHHSignins { get; set; }
        public int dispatchedDWCSignins { get; set; }
        public int dispatchedHHHSignins { get; set; }
        public int totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
    }
}