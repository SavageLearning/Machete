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

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Data.Initialize
{
    /// <summary>
    /// <para>Machete.Data.Initialize.MacheteConfiguration class.</para>
    /// <para>This class is responsible for ensuring the presence of the Seed data needed to run the application.</para>
    /// </summary>
    public static class MacheteConfiguration
    {
        public static void Seed(MacheteContext db)
        {
            if (!db.Lookups.Any())
                MacheteLookups.Initialize(db);
            if (!db.TransportProviders.Any() || !db.TransportProvidersAvailability.Any())
                MacheteTransports.Initialize(db);
            if (!db.Configs.Any())
                MacheteConfigs.Initialize(db);
            if (!db.TransportRules.Any())
                MacheteRules.Initialize(db);
            if (db.ReportDefinitions.Count() != MacheteReportDefinitions._cache.Count)
                MacheteReportDefinitions.Initialize(db);
        }

        public static async Task SeedAsync(MacheteContext db,
            RoleManager<IdentityRole> roleManager, UserManager<MacheteUser> userManager)
        {
            if (!db.Users.Any())
                await MacheteUsers.Initialize(roleManager, userManager);
        }
    }
}