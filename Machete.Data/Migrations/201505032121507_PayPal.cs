namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayPal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "paypalFee", c => c.Double());
            AddColumn("dbo.WorkOrders", "paypalCorrelationId", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "paypalToken", c => c.String(maxLength: 20));
            AddColumn("dbo.WorkOrders", "paypalPayerId", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "paypalPayerId");
            DropColumn("dbo.WorkOrders", "paypalToken");
            DropColumn("dbo.WorkOrders", "paypalCorrelationId");
            DropColumn("dbo.WorkOrders", "paypalFee");
        }
    }
}
