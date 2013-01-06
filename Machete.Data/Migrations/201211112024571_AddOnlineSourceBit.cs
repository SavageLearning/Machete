#region COPYRIGHT
// File:     201211112024571_AddOnlineSourceBit.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
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
namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddOnlineSourceBit : DbMigration
    {
        public override void Up()
        {
            AddColumn("WorkOrders", "onlineSource", c => c.Boolean(nullable: false));
            AddColumn("Employers", "onlineSource", c => c.Boolean(nullable: false));
            AddColumn("Employers", "returnCustomer", c => c.Boolean(nullable: false));
            AddColumn("Employers", "receiveUpdates", c => c.Boolean(nullable: false));
            AlterColumn("WorkerSignins", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("ActivitySignins", "ID", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("ActivitySignins", "ID", c => c.Int(nullable: false));
            AlterColumn("WorkerSignins", "ID", c => c.Int(nullable: false));
            DropColumn("Employers", "receiveUpdates");
            DropColumn("Employers", "returnCustomer");
            DropColumn("Employers", "onlineSource");
            DropColumn("WorkOrders", "onlineSource");
        }
    }
}
