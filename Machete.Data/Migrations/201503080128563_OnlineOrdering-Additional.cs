namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnlineOrderingAdditional : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkAssignments", "weightLifted", c => c.Boolean());
            AddColumn("dbo.WorkOrders", "additionalNotes", c => c.String(maxLength: 1000));
            AddColumn("dbo.WorkOrders", "disclosureAgreement", c => c.Boolean());
            DropColumn("dbo.WorkAssignments", "additionalNotes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkAssignments", "additionalNotes", c => c.String(maxLength: 1000));
            DropColumn("dbo.WorkOrders", "disclosureAgreement");
            DropColumn("dbo.WorkOrders", "additionalNotes");
            DropColumn("dbo.WorkAssignments", "weightLifted");
        }
    }
}
