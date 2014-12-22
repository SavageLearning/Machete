namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransportTransactionData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "transportTransactID", c => c.String(maxLength: 50, nullable: true));
            AddColumn("dbo.WorkOrders", "transportTransactType", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "transportTransactID");
            DropColumn("dbo.WorkOrders", "transportTransactType");
        }
    }
}
