namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployerLicenseMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employers", "licenseplate", c => c.String(maxLength: 10));
            AddColumn("dbo.Employers", "driverslicense", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employers", "driverslicense");
            DropColumn("dbo.Employers", "licenseplate");
        }
    }
}
