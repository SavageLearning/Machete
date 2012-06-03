namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ActivityModelChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ActivitySignins", "WorkerID", "Workers");
            DropForeignKey("ActivitySignins", "activityID", "Activities");
            DropIndex("ActivitySignins", new[] { "WorkerID" });
            DropIndex("ActivitySignins", new[] { "activityID" });
            AddColumn("ActivitySignins", "personID", c => c.Int());
            AlterColumn("ActivitySignins", "activityID", c => c.Int(nullable: false));
            AddForeignKey("ActivitySignins", "personID", "Persons", "ID");
            AddForeignKey("ActivitySignins", "activityID", "Activities", "ID", cascadeDelete: true);
            CreateIndex("ActivitySignins", "personID");
            CreateIndex("ActivitySignins", "activityID");
            DropColumn("ActivitySignins", "WorkerID");
        }
        
        public override void Down()
        {
            AddColumn("ActivitySignins", "WorkerID", c => c.Int());
            DropIndex("ActivitySignins", new[] { "activityID" });
            DropIndex("ActivitySignins", new[] { "personID" });
            DropForeignKey("ActivitySignins", "activityID", "Activities");
            DropForeignKey("ActivitySignins", "personID", "Persons");
            AlterColumn("ActivitySignins", "activityID", c => c.Int(nullable: false));
            DropColumn("ActivitySignins", "personID");
            CreateIndex("ActivitySignins", "activityID");
            CreateIndex("ActivitySignins", "WorkerID");
            AddForeignKey("ActivitySignins", "activityID", "Activities", "ID", cascadeDelete: true);
            AddForeignKey("ActivitySignins", "WorkerID", "Workers", "ID");
        }
    }
}
