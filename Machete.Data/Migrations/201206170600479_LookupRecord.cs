namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LookupRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("Lookups", "datecreated", c => c.DateTime(nullable: false));
            AddColumn("Lookups", "dateupdated", c => c.DateTime(nullable: false));
            AddColumn("Lookups", "Createdby", c => c.String(maxLength: 30));
            AddColumn("Lookups", "Updatedby", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("Lookups", "Updatedby");
            DropColumn("Lookups", "Createdby");
            DropColumn("Lookups", "dateupdated");
            DropColumn("Lookups", "datecreated");
        }
    }
}
