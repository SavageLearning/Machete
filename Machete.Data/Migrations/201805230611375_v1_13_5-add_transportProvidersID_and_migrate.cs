namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_5add_transportProvidersID_and_migrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "transportProviderID", c => c.Int(nullable: false));

            Sql(@"update workorders
set workorders.transportproviderid = tp.id
from workorders wo
join lookups l on l.id = wo.transportMethodID
join transportproviders tp on tp.[key] = l.[key]");

        }



        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "transportProviderID");
        }
    }
}
