namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ActivitiesAndHomelessBit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        dateStart = c.DateTime(nullable: false),
                        dateEnd = c.DateTime(nullable: false),
                        teacher = c.String(nullable: false),
                        notes = c.String(maxLength: 4000),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ActivitySignins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActivityID = c.Int(nullable: false),
                        dwccardnum = c.Int(nullable: false),
                        WorkerID = c.Int(),
                        memberStatus = c.Int(),
                        WorkAssignmentID = c.Int(),
                        dateforsignin = c.DateTime(nullable: false),
                        lottery_timestamp = c.DateTime(),
                        lottery_sequence = c.Int(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Workers", t => t.WorkerID)
                .ForeignKey("Activities", t => t.ActivityID, cascadeDelete: true)
                .Index(t => t.WorkerID)
                .Index(t => t.ActivityID);
            
            AddColumn("Workers", "homeless", c => c.Boolean());
            AddColumn("WorkerSignins", "memberStatus", c => c.Int());
            AddColumn("WorkerSignins", "lottery_sequence", c => c.Int());
            AlterColumn("Events", "dateTo", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropIndex("ActivitySignins", new[] { "ActivityID" });
            DropIndex("ActivitySignins", new[] { "WorkerID" });
            DropForeignKey("ActivitySignins", "ActivityID", "Activities");
            DropForeignKey("ActivitySignins", "WorkerID", "Workers");
            AlterColumn("Events", "dateTo", c => c.DateTime(nullable: false));
            DropColumn("WorkerSignins", "lottery_sequence");
            DropColumn("WorkerSignins", "memberStatus");
            DropColumn("Workers", "homeless");
            DropTable("ActivitySignins");
            DropTable("Activities");
        }
    }
}
