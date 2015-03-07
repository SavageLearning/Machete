namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RaceIDNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Workers", "RaceID", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            AlterColumn("dbo.Workers", "RaceID", c => c.Int(nullable: false));
        }
    }
}
