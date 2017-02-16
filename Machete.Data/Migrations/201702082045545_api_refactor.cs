namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class api_refactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "nameEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "nameES", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeES", c => c.String(maxLength: 50));
            AddColumn("dbo.Persons", "fullName", c => c.String());
            AddColumn("dbo.Events", "eventTypeEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Events", "eventTypeES", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "typeOfWork", c => c.String());
            AddColumn("dbo.Workers", "memberStatusEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "memberStatusES", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "skillCodes", c => c.String());
            AddColumn("dbo.WorkAssignments", "skillEN", c => c.String());
            AddColumn("dbo.WorkAssignments", "skillES", c => c.String());
            AddColumn("dbo.WorkAssignments", "fullWAID", c => c.String());
            AddColumn("dbo.WorkAssignments", "minEarnings", c => c.Double(nullable: false));
            AddColumn("dbo.WorkAssignments", "maxEarnings", c => c.Double(nullable: false));
            AddColumn("dbo.WorkOrders", "statusEN", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "statusES", c => c.String(maxLength: 50));
            AddColumn("dbo.Lookups", "active", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lookups", "active");
            DropColumn("dbo.WorkOrders", "statusES");
            DropColumn("dbo.WorkOrders", "statusEN");
            DropColumn("dbo.WorkAssignments", "maxEarnings");
            DropColumn("dbo.WorkAssignments", "minEarnings");
            DropColumn("dbo.WorkAssignments", "fullWAID");
            DropColumn("dbo.WorkAssignments", "skillES");
            DropColumn("dbo.WorkAssignments", "skillEN");
            DropColumn("dbo.Workers", "skillCodes");
            DropColumn("dbo.Workers", "memberStatusES");
            DropColumn("dbo.Workers", "memberStatusEN");
            DropColumn("dbo.Workers", "typeOfWork");
            DropColumn("dbo.Events", "eventTypeES");
            DropColumn("dbo.Events", "eventTypeEN");
            DropColumn("dbo.Persons", "fullName");
            DropColumn("dbo.Activities", "typeES");
            DropColumn("dbo.Activities", "typeEN");
            DropColumn("dbo.Activities", "nameES");
            DropColumn("dbo.Activities", "nameEN");
        }
    }
}
