namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_0online_orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configs", "publicConfig", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Lookups", "clientRules", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            Sql(@"update lookups set [key] = 'transport_bus' where id = 29");
            Sql(@"update lookups set [key] = 'transport_car' where id = 30");
            Sql(@"update lookups set [key] = 'transport_pickup' where id = 31");
            Sql(@"update lookups set [key] = 'transport_van' where id = 32");
            Sql(@"update configs set publicConfig = 0 where category = 'Emails'");
            Sql(@"update configs set publicConfig = 0 where [key] = 'PayPalAccountID'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Lookups", "clientRules");
            DropColumn("dbo.Configs", "publicConfig");
        }
    }
}
