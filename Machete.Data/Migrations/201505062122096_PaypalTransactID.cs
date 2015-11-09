namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaypalTransactID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "paypalTransactID", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "paypalTransactID");
        }
    }
}
