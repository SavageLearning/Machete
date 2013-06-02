namespace Machete.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JoinWorkorderEmails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkOrderID = c.Int(nullable: false),
                        EmailID = c.Int(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderID, cascadeDelete: true)
                .ForeignKey("dbo.Emails", t => t.EmailID, cascadeDelete: true)
                .Index(t => t.WorkOrderID)
                .Index(t => t.EmailID);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        emailFrom = c.String(nullable: false, maxLength: 50),
                        emailTo = c.String(nullable: false, maxLength: 50),
                        subject = c.String(nullable: false, maxLength: 100),
                        body = c.String(nullable: false),
                        transmitAttempts = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        lastAttempt = c.DateTime(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Lookups", "emailTemplate", c => c.String());
            AddColumn("dbo.Lookups", "key", c => c.String(maxLength: 30));
            AlterColumn("dbo.Lookups", "ltrCode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropIndex("dbo.JoinWorkorderEmails", new[] { "EmailID" });
            DropIndex("dbo.JoinWorkorderEmails", new[] { "WorkOrderID" });
            DropForeignKey("dbo.JoinWorkorderEmails", "EmailID", "dbo.Emails");
            DropForeignKey("dbo.JoinWorkorderEmails", "WorkOrderID", "dbo.WorkOrders");
            AlterColumn("dbo.Lookups", "ltrCode", c => c.String(maxLength: 1));
            DropColumn("dbo.Lookups", "key");
            DropColumn("dbo.Lookups", "emailTemplate");
            DropTable("dbo.Emails");
            DropTable("dbo.JoinWorkorderEmails");
        }
    }
}
