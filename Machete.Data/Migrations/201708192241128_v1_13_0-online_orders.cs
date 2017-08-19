namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_0online_orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lookups", "clientRules", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lookups", "clientRules");
        }
    }
}
