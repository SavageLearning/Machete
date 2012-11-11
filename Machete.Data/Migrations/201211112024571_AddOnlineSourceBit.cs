namespace Machete.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOnlineSourceBit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "onlineSource", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employers", "onlineSource", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employers", "onlineSource");
            DropColumn("dbo.WorkOrders", "onlineSource");
        }
    }
}
