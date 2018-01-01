namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_0online_orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScheduleRules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        day = c.Int(nullable: false),
                        leadHours = c.Int(nullable: false),
                        minStartMin = c.Int(nullable: false),
                        maxEndMin = c.Int(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransportCostRules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        transportRuleID = c.Int(nullable: false),
                        minWorker = c.Int(nullable: false),
                        maxWorker = c.Int(nullable: false),
                        cost = c.Double(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TransportRules", t => t.transportRuleID, cascadeDelete: true)
                .Index(t => t.transportRuleID);
            
            CreateTable(
                "dbo.TransportRules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(maxLength: 50),
                        lookupKey = c.String(maxLength: 50),
                        zoneLabel = c.String(maxLength: 50),
                        zipcodes = c.String(maxLength: 1000),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.WorkAssignments", "transportCost", c => c.Double());
            AddColumn("dbo.Configs", "publicConfig", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            Sql(@"update configs set publicConfig = 0 where category = 'Emails'");
            Sql(@"update configs set publicConfig = 0 where [key] = 'PayPalAccountID'");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportCostRules", "transportRuleID", "dbo.TransportRules");
            DropIndex("dbo.TransportCostRules", new[] { "transportRuleID" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Configs", "publicConfig");
            DropColumn("dbo.WorkAssignments", "transportCost");
            DropTable("dbo.TransportRules");
            DropTable("dbo.TransportCostRules");
            DropTable("dbo.ScheduleRules");
        }
    }
}
