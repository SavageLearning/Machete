namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_13_1replace_paypal_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "ppFee", c => c.Double());
            AddColumn("dbo.WorkOrders", "ppResponse", c => c.String());
            AddColumn("dbo.WorkOrders", "ppPaymentToken", c => c.String(maxLength: 25));
            AddColumn("dbo.WorkOrders", "ppPaymentID", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "ppPayerID", c => c.String(maxLength: 25));
            AddColumn("dbo.WorkOrders", "ppState", c => c.String(maxLength: 20));
            DropColumn("dbo.WorkOrders", "paypalFee");
            DropColumn("dbo.WorkOrders", "paypalErrors");
            DropColumn("dbo.WorkOrders", "paypalToken");
            DropColumn("dbo.WorkOrders", "paypalTransactID");
            DropColumn("dbo.WorkOrders", "paypalPayerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrders", "paypalPayerId", c => c.String(maxLength: 15));
            AddColumn("dbo.WorkOrders", "paypalTransactID", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "paypalToken", c => c.String(maxLength: 20));
            AddColumn("dbo.WorkOrders", "paypalErrors", c => c.String(maxLength: 1000));
            AddColumn("dbo.WorkOrders", "paypalFee", c => c.Double());
            DropColumn("dbo.WorkOrders", "ppState");
            DropColumn("dbo.WorkOrders", "ppPayerID");
            DropColumn("dbo.WorkOrders", "ppPaymentID");
            DropColumn("dbo.WorkOrders", "ppPaymentToken");
            DropColumn("dbo.WorkOrders", "ppResponse");
            DropColumn("dbo.WorkOrders", "ppFee");
        }
    }
}
