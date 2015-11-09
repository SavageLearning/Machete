namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkDemographics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Persons", "nickname", c => c.String(maxLength: 50));
            AddColumn("dbo.Persons", "cellphone", c => c.String(maxLength: 12));
            AddColumn("dbo.Persons", "email", c => c.String(maxLength: 50));
            AddColumn("dbo.Persons", "facebook", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "housingType", c => c.Int());
            AddColumn("dbo.Workers", "liveWithSpouse", c => c.Boolean());
            AddColumn("dbo.Workers", "liveWithDescription", c => c.String(maxLength: 1000));
            AddColumn("dbo.Workers", "americanBornChildren", c => c.Int());
            AddColumn("dbo.Workers", "numChildrenUnder18", c => c.Int());
            AddColumn("dbo.Workers", "educationLevel", c => c.Int());
            AddColumn("dbo.Workers", "farmLaborCharacteristics", c => c.Int());
            AddColumn("dbo.Workers", "wageTheftVictim", c => c.Boolean());
            AddColumn("dbo.Workers", "wageTheftRecoveryAmount", c => c.Double());
            AddColumn("dbo.Workers", "lastPaymentDate", c => c.DateTime());
            AddColumn("dbo.Workers", "lastPaymentAmount", c => c.Double());
            AddColumn("dbo.Workers", "ownTools", c => c.Boolean());
            AddColumn("dbo.Workers", "healthInsurance", c => c.Boolean());
            AddColumn("dbo.Workers", "usVeteran", c => c.Boolean());
            AddColumn("dbo.Workers", "healthInsuranceDate", c => c.DateTime());
            AddColumn("dbo.Workers", "vehicleTypeID", c => c.Int());
            AddColumn("dbo.Workers", "incomeSourceID", c => c.Int());
            AddColumn("dbo.Workers", "introToCenter", c => c.String(maxLength: 1000));
            AddColumn("dbo.Workers", "lgbtq", c => c.Boolean());
            AddColumn("dbo.WorkOrders", "paypalErrors", c => c.String(maxLength: 1000));
            AddColumn("dbo.Employers", "fax", c => c.String(maxLength: 12));
            AddColumn("dbo.Lookups", "skillDescriptionEn", c => c.String(maxLength: 300));
            AddColumn("dbo.Lookups", "skillDescriptionEs", c => c.String(maxLength: 300));
            AddColumn("dbo.Lookups", "minimumCost", c => c.Double());
            AlterColumn("dbo.Workers", "dateOfBirth", c => c.DateTime());
            AlterColumn("dbo.Workers", "active", c => c.Boolean());
            AlterColumn("dbo.Workers", "RaceID", c => c.Int());
            AlterColumn("dbo.Workers", "height", c => c.String(maxLength: 50));
            AlterColumn("dbo.Workers", "weight", c => c.String(maxLength: 10));
            AlterColumn("dbo.Workers", "recentarrival", c => c.Boolean());
            AlterColumn("dbo.Workers", "dateinUSA", c => c.DateTime());
            AlterColumn("dbo.Workers", "dateinseattle", c => c.DateTime());
            AlterColumn("dbo.Workers", "disabled", c => c.Boolean());
            AlterColumn("dbo.Workers", "maritalstatus", c => c.Int());
            AlterColumn("dbo.Workers", "livewithchildren", c => c.Boolean());
            AlterColumn("dbo.Workers", "numofchildren", c => c.Int());
            AlterColumn("dbo.Workers", "incomeID", c => c.Int());
            AlterColumn("dbo.Workers", "livealone", c => c.Boolean());
            AlterColumn("dbo.Workers", "neighborhoodID", c => c.Int());
            AlterColumn("dbo.Workers", "immigrantrefugee", c => c.Boolean());
            AlterColumn("dbo.Workers", "countryoforiginID", c => c.Int());
            AlterColumn("dbo.Workers", "driverslicense", c => c.Boolean());
            AlterColumn("dbo.Employers", "blogparticipate", c => c.Boolean());
            DropColumn("dbo.WorkOrders", "paypalCorrelationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrders", "paypalCorrelationId", c => c.String(maxLength: 50));
            AlterColumn("dbo.Employers", "blogparticipate", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "driverslicense", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "countryoforiginID", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "immigrantrefugee", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "neighborhoodID", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "livealone", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "incomeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "numofchildren", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "livewithchildren", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "maritalstatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "disabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "dateinseattle", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Workers", "dateinUSA", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Workers", "recentarrival", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "weight", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Workers", "height", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Workers", "RaceID", c => c.Int(nullable: false));
            AlterColumn("dbo.Workers", "active", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Workers", "dateOfBirth", c => c.DateTime(nullable: false));
            DropColumn("dbo.Lookups", "minimumCost");
            DropColumn("dbo.Lookups", "skillDescriptionEs");
            DropColumn("dbo.Lookups", "skillDescriptionEn");
            DropColumn("dbo.Employers", "fax");
            DropColumn("dbo.WorkOrders", "paypalErrors");
            DropColumn("dbo.Workers", "lgbtq");
            DropColumn("dbo.Workers", "introToCenter");
            DropColumn("dbo.Workers", "incomeSourceID");
            DropColumn("dbo.Workers", "vehicleTypeID");
            DropColumn("dbo.Workers", "healthInsuranceDate");
            DropColumn("dbo.Workers", "usVeteran");
            DropColumn("dbo.Workers", "healthInsurance");
            DropColumn("dbo.Workers", "ownTools");
            DropColumn("dbo.Workers", "lastPaymentAmount");
            DropColumn("dbo.Workers", "lastPaymentDate");
            DropColumn("dbo.Workers", "wageTheftRecoveryAmount");
            DropColumn("dbo.Workers", "wageTheftVictim");
            DropColumn("dbo.Workers", "farmLaborCharacteristics");
            DropColumn("dbo.Workers", "educationLevel");
            DropColumn("dbo.Workers", "numChildrenUnder18");
            DropColumn("dbo.Workers", "americanBornChildren");
            DropColumn("dbo.Workers", "liveWithDescription");
            DropColumn("dbo.Workers", "liveWithSpouse");
            DropColumn("dbo.Workers", "housingType");
            DropColumn("dbo.Persons", "facebook");
            DropColumn("dbo.Persons", "email");
            DropColumn("dbo.Persons", "cellphone");
            DropColumn("dbo.Persons", "nickname");
        }
    }
}
