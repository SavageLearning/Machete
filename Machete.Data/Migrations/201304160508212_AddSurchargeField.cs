namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddSurchargeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("WorkAssignments", "surcharge", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("WorkAssignments", "surcharge");
        }
    }
}
