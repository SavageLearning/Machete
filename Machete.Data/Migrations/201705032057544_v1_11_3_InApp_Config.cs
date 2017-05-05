namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_11_3_InApp_Config : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(nullable: false, maxLength: 50),
                        value = c.String(nullable: false),
                        description = c.String(),
                        category = c.String(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ActivitySignins", "timeZoneOffset", c => c.Double(nullable: false));
            AddColumn("dbo.WorkerSignins", "timeZoneOffset", c => c.Double(nullable: false));
            AddColumn("dbo.WorkOrders", "timeZoneOffset", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "timeZoneOffset");
            DropColumn("dbo.WorkerSignins", "timeZoneOffset");
            DropColumn("dbo.ActivitySignins", "timeZoneOffset");
            DropTable("dbo.Configs");
        }
    }
}
