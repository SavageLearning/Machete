namespace Machete.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        emailFrom = c.String(maxLength: 50),
                        emailTo = c.String(nullable: false, maxLength: 50),
                        subject = c.String(nullable: false, maxLength: 100),
                        body = c.String(nullable: false),
                        transmitAttempts = c.Int(nullable: false),
                        statusID = c.Int(nullable: false),
                        lastAttempt = c.DateTime(),
                        attachment = c.String(),
                        attachmentContentType = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EmailWorkOrders",
                c => new
                    {
                        Email_ID = c.Int(nullable: false),
                        WorkOrder_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Email_ID, t.WorkOrder_ID })
                .ForeignKey("dbo.Emails", t => t.Email_ID, cascadeDelete: true)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrder_ID, cascadeDelete: true)
                .Index(t => t.Email_ID)
                .Index(t => t.WorkOrder_ID);
            
            AddColumn("dbo.Lookups", "emailTemplate", c => c.String());
            AddColumn("dbo.Lookups", "key", c => c.String(maxLength: 30));
            AlterColumn("dbo.Lookups", "ltrCode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropIndex("dbo.EmailWorkOrders", new[] { "WorkOrder_ID" });
            DropIndex("dbo.EmailWorkOrders", new[] { "Email_ID" });
            DropForeignKey("dbo.EmailWorkOrders", "WorkOrder_ID", "dbo.WorkOrders");
            DropForeignKey("dbo.EmailWorkOrders", "Email_ID", "dbo.Emails");
            AlterColumn("dbo.Lookups", "ltrCode", c => c.String(maxLength: 1));
            DropColumn("dbo.Lookups", "key");
            DropColumn("dbo.Lookups", "emailTemplate");
            DropTable("dbo.EmailWorkOrders");
            DropTable("dbo.Emails");
        }
    }
}
