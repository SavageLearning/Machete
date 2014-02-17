namespace Machete.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecurringBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "recurring", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "recurring");
        }
    }
}
