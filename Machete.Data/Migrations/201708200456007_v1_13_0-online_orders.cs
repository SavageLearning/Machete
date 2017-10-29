namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_0online_orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkAssignments", "transportCost", c => c.Double());
            AddColumn("dbo.Configs", "publicConfig", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            Sql(@"update configs set publicConfig = 0 where category = 'Emails'");
            Sql(@"update configs set publicConfig = 0 where [key] = 'PayPalAccountID'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Configs", "publicConfig");
            DropColumn("dbo.WorkAssignments", "transportCost");
        }
    }
}
