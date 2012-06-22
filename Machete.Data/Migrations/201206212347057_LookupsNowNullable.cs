namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LookupsNowNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Lookups", "category", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("Lookups", "text_EN", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Lookups", "text_ES", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Lookups", "subcategory", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("Lookups", "subcategory", c => c.String(maxLength: 50));
            AlterColumn("Lookups", "text_ES", c => c.String(maxLength: 50));
            AlterColumn("Lookups", "text_EN", c => c.String(maxLength: 50));
            AlterColumn("Lookups", "category", c => c.String(maxLength: 50));
        }
    }
}
