namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnlineOrdering : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "workerRating", c => c.Single());
            AddColumn("dbo.WorkAssignments", "workerRating", c => c.Int());
            AddColumn("dbo.WorkAssignments", "workerRatingComments", c => c.String(maxLength: 500));
            AddColumn("dbo.WorkAssignments", "additionalNotes", c => c.String(maxLength: 1000));
            AddColumn("dbo.Employers", "onlineSigninID", c => c.String(maxLength: 128));
            AddColumn("dbo.Employers", "isOnlineProfileComplete", c => c.Boolean());
        }

        public override void Down()
        {
            DropColumn("dbo.Employers", "isOnlineProfileComplete");
            DropColumn("dbo.Employers", "onlineSigninID");
            DropColumn("dbo.WorkAssignments", "additionalNotes");
            DropColumn("dbo.WorkAssignments", "workerRatingComments");
            DropColumn("dbo.WorkAssignments", "workerRating");
            DropColumn("dbo.Workers", "workerRating");
        }
    }
}
