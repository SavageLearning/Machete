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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Machete.Data
{
    //public class MacheteInitializer : DropCreateDatabaseIfModelChanges<MacheteContext>
    public class MacheteInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration>
    {

    }
    public class TestInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration>
    {

    }

    public class MacheteConfiguration : DbMigrationsConfiguration<MacheteContext>
    {
        public MacheteConfiguration()
            : base()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookups
            if (!DB.Lookups.Any())
            {
                MacheteLookup.Initialize(DB);
                DB.SaveChanges();
            }

            if (!DB.Users.Any())
            {
                AddUserAndRole(DB);
            }
        }


        bool AddUserAndRole(MacheteContext context)
        {
            IdentityResult ir;

            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("Administrator"));
            ir = rm.Create(new IdentityRole("Manager"));
            ir = rm.Create(new IdentityRole("Check-in"));
            ir = rm.Create(new IdentityRole("PhoneDesk"));
            ir = rm.Create(new IdentityRole("Teacher"));
            ir = rm.Create(new IdentityRole("User"));
            ir = rm.Create(new IdentityRole("Hirer")); // This role is used exclusively for the online hiring interface

            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "jadmin",
                IsApproved = true,
                Email = "here@there.org"
            };
            
            ir = um.Create(user, "ChangeMe");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Administrator"); //Default Administrator, edit to change
            ir = um.AddToRole(user.Id, "Teacher"); //Required to make tests work
            return ir.Succeeded;
        }
    }
}
