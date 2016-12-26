namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activity_lookup_strings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "nameEN", c => c.String());
            AddColumn("dbo.Activities", "nameES", c => c.String());
            AddColumn("dbo.Activities", "typeEN", c => c.String());
            AddColumn("dbo.Activities", "typeES", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "typeES");
            DropColumn("dbo.Activities", "typeEN");
            DropColumn("dbo.Activities", "nameES");
            DropColumn("dbo.Activities", "nameEN");
        }
    }
}
