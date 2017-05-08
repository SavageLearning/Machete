namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_12_0_reports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportDefinitions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        sqlquery = c.String(),
                        category = c.String(),
                        subcategory = c.String(),
                        columnLabelsJson = c.String(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReportDefinitions");
        }
    }
}
