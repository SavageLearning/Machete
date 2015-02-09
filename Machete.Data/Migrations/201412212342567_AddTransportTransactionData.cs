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
            Sql("INSERT INTO Lookups(category, text_EN, text_ES, selected, speciality, dateCreated, dateUpdated, createdBy, updatedBy) VALUES('transportPaymentType','Cash','Cash', 1, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), ('transportPaymentType','Paypal','Paypal', 1, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), ('transportPaymentType','Check','Check', 1, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), ('transportPaymentType','Credit Card','Credit Card', 1, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration');");
        }

        public override void Down()
        {
            // ALTER TABLE dbo.WorkOrders DROP COLUMN transportTransactID;
            DropColumn("dbo.WorkOrders", "transportTransactID");
            // ALTER TABLE dbo.WorkOrders DROP COLUMN transportTransactType;
            DropColumn("dbo.WorkOrders", "transportTransactType");
            // DELETE FROM Lookups WHERE category='transportPaymentType';
            Sql("DELETE FROM Lookups WHERE category='transportPaymentType';");
        }
    }
}
