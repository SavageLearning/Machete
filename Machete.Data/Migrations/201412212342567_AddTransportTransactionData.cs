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
            Sql("SET IDENTITY_INSERT dbo.Lookups ON;");
            Sql("INSERT INTO Lookups(ID, category, text_EN, text_ES, selected, speciality, dateCreated, dateUpdated, createdBy, updatedBy) VALUES(255, 'transportPaymentType','Cash','Cash', 1, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), (256, 'transportPaymentType','Paypal','Paypal', 0, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), (257, 'transportPaymentType','Check','Check', 0, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration'), (258, 'transportPaymentType','Credit Card','Credit Card', 0, 0, getDate(), getDate(), 'DB.Migration', 'DB.Migration');");
            Sql("SET IDENTITY_INSERT dbo.Lookups OFF;");
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
