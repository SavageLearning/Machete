namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransportTransactionData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "transportTransactType", c => c.Int());
            AddColumn("dbo.WorkOrders", "transportTransactID", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            // ALTER TABLE dbo.WorkOrders DROP COLUMN transportTransactID;
            DropColumn("dbo.WorkOrders", "transportTransactID");
            // ALTER TABLE dbo.WorkOrders DROP COLUMN transportTransactType;
            DropColumn("dbo.WorkOrders", "transportTransactType");
        }

        
    }
}
