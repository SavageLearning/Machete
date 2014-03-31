namespace Machete.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employers", "businessname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employers", "businessname");
        }
    }
}
