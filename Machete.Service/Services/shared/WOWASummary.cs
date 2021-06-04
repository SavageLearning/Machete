#region COPYRIGHT
// File:     AllRepositories.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
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
using Machete.Service.Infrastructure;
using Machete.Domain;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace Machete.Service
{

    public class WOWASummary
    {
        public string date { get; set; }
        public DateTime sortableDate { get; set;}
        public string weekday { get; set; }
        public int? PendingWO { get; set; }
        public int? PendingWA { get; set; }
        public int? ActiveWO { get; set; }
        public int? ActiveWA { get; set; }
        public int? CompletedWO { get; set; }
        public int? CompletedWA { get; set; }
        public int? CancelledWO { get; set; }
        public int? CancelledWA { get; set; }
        public int? ExpiredWO { get; set; }
        public int? ExpiredWA { get; set; }
    }
}

