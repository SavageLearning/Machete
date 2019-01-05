#region COPYRIGHT
// File:     MacheteInitializer.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
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
using System.Linq;
using Machete.Data.Initialize;

namespace Machete.Data
{
    public static class MacheteConfiguration
    {
        public static void Seed(MacheteContext db, IServiceProvider services)
        {
            if (db.Lookups.Count() == 0) MacheteLookup.Initialize(db);
            if (db.TransportProviders.Count() == 0 || db.TransportProvidersAvailability.Count() == 0) MacheteTransports.Initialize(db);
            if (db.Users.Count() == 0) MacheteUsers.Initialize(services);
            // MacheteConfigs.Initialize assumes Configs table has been populated by script
            if (db.Configs.Count() == 0) MacheteConfigs.Initialize(db);
            if (db.TransportRules.Count() == 0) MacheteRules.Initialize(db);
            if (db.ReportDefinitions.Count() != MacheteReportDefinitions.cache.Count) MacheteReportDefinitions.Initialize(db);
        }
    }   
}
