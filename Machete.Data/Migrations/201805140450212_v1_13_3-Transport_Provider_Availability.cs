namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_3Transport_Provider_Availability : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportProviders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(maxLength: 50),
                        text_EN = c.String(maxLength: 50),
                        text_ES = c.String(maxLength: 50),
                        defaultAttribute = c.Boolean(nullable: false),
                        sortorder = c.Int(),
                        active = c.Boolean(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransportProviderAvailabilities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        transportProviderID = c.Int(nullable: false),
                        key = c.String(maxLength: 50),
                        lookupKey = c.String(maxLength: 50),
                        day = c.Int(nullable: false),
                        available = c.Boolean(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TransportProviders", t => t.transportProviderID, cascadeDelete: true)
                .Index(t => t.transportProviderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportProviderAvailabilities", "transportProviderID", "dbo.TransportProviders");
            DropIndex("dbo.TransportProviderAvailabilities", new[] { "transportProviderID" });
            DropTable("dbo.TransportProviderAvailabilities");
            DropTable("dbo.TransportProviders");
        }
    }
}
