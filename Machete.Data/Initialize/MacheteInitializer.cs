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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
using System.Data.Entity.Migrations;

namespace Machete.Data
{
    //public class MacheteInitializer : DropCreateDatabaseIfModelChanges<MacheteContext>
    public class MacheteInitializer : CreateDatabaseIfNotExists<MacheteContext>
    {
        
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookups
            MacheteLookup.Initialize(DB);
            DB.SaveChanges();
            DB.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX [dateTimeofWork] ON [dbo].[WorkOrders] ([dateTimeofWork] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
        }
    }
    public class TestInitializer : DropCreateDatabaseAlways<MacheteContext>
    {
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookup tables with static data
            MacheteLookup.Initialize(DB); //Adds the Lookups table records
            DB.SaveChanges();
            DB.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX [dateTimeofWork] ON [dbo].[WorkOrders] ([dateTimeofWork] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
        }
    }

    //public class CustomMigrationsConfiguration : DbMigrationsConfiguration<MacheteContext>
    //{
    //    public CustomMigrationsConfiguration()
    //        : base()
    //    {
    //        AutomaticMigrationsEnabled = true;
    //        AutomaticMigrationDataLossAllowed = true;
    //    }

    //    //protected override void Seed(MacheteContext context)
    //    //{
    //    //}
    //}
}
