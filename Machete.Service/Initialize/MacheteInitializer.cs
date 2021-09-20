#region COPYRIGHT
// File:     MacheteInitializer.cs
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

using System.Linq;
using System.Threading.Tasks;
using Machete.Service.Identity;
using Machete.Service.Infrastructure;
using Machete.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Service.Initialize
{
    /// <summary>
    /// <para>Machete.Service.Initialize.MacheteConfiguration class.</para>
    /// <para>This class is responsible for ensuring the presence of the Seed data needed to run the application.</para>
    /// </summary>
    public static class MacheteConfiguration
    {
        public static void Seed(MacheteContext db, string tenantTimeZone)
        {
            if (!db.Lookups.Any())
                MacheteLookups.Initialize(db);
            if (!db.TransportProviders.Any() || !db.TransportProviderAvailabilities.Any())
                MacheteTransports.Initialize(db);
            MacheteConfigs.Synchronize(db, tenantTimeZone);
            if (!db.TransportRules.Any())
                MacheteRules.Initialize(db);
            if (db.ReportDefinitions.Count() == 0) // Reports are manged in UI now
                MacheteReportDefinitions.Initialize(db);
        }

        public static async Task SeedAsync(MacheteContext db)
        {
            if (!db.Users.Any())
                await MacheteSeedUsers.Initialize(db);
        }
    }
}
