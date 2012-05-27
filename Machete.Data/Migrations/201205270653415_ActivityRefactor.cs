namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ActivityRefactor : DbMigration
    {
        public override void Up()
        {
            DropColumn("ActivitySignins", "WorkAssignmentID");
            DropColumn("ActivitySignins", "lottery_timestamp");
            DropColumn("ActivitySignins", "lottery_sequence");
        }
        
        public override void Down()
        {
            AddColumn("ActivitySignins", "lottery_sequence", c => c.Int());
            AddColumn("ActivitySignins", "lottery_timestamp", c => c.DateTime());
            AddColumn("ActivitySignins", "WorkAssignmentID", c => c.Int());
        }
    }
}
