namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddOnlineSourceBit : DbMigration
    {
        public override void Up()
        {
            AddColumn("WorkOrders", "onlineSource", c => c.Boolean(nullable: false));
            AddColumn("Employers", "onlineSource", c => c.Boolean(nullable: false));
            AddColumn("Employers", "returnCustomer", c => c.Boolean(nullable: false));
            AddColumn("Employers", "receiveUpdates", c => c.Boolean(nullable: false));
            AlterColumn("WorkerSignins", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("ActivitySignins", "ID", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("ActivitySignins", "ID", c => c.Int(nullable: false));
            AlterColumn("WorkerSignins", "ID", c => c.Int(nullable: false));
            DropColumn("Employers", "receiveUpdates");
            DropColumn("Employers", "returnCustomer");
            DropColumn("Employers", "onlineSource");
            DropColumn("WorkOrders", "onlineSource");
        }
    }
}
