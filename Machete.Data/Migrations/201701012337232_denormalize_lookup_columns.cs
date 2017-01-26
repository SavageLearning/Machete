namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class denormalize_lookup_columns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "memberStatusEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "memberStatusES", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "nameEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "nameES", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeES", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "typeES");
            DropColumn("dbo.Activities", "typeEN");
            DropColumn("dbo.Activities", "nameES");
            DropColumn("dbo.Activities", "nameEN");
            DropColumn("dbo.Workers", "memberStatusES");
            DropColumn("dbo.Workers", "memberStatusEN");
        }
    }
}
