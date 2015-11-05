namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterHoursTypeIntegerToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkAssignments", "hours", c => c.Double(nullable: false));
        
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkAssignments", "hours", c => c.Int());

        }
    }
}
