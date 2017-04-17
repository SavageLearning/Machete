﻿#region COPYRIGHT
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
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Machete.Data
{
    public class MacheteInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration> {}
    public class TestInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration> {}

    public class MacheteConfiguration : DbMigrationsConfiguration<MacheteContext>
    {
        public MacheteConfiguration() : base()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(MacheteContext DB)
        {
            if (DB.Lookups.Count() == 0) MacheteLookup.Initialize(DB);
            if (DB.Users.Count() == 0)   MacheteUsers.Initialize(DB);
        }
    }   
}
