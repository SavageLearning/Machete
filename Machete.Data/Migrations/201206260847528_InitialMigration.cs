namespace Machete.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "Persons",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            active = c.Boolean(nullable: false),
            //            firstname1 = c.String(nullable: false, maxLength: 50),
            //            firstname2 = c.String(maxLength: 50),
            //            lastname1 = c.String(nullable: false, maxLength: 50),
            //            lastname2 = c.String(maxLength: 50),
            //            address1 = c.String(maxLength: 50),
            //            address2 = c.String(maxLength: 50),
            //            city = c.String(maxLength: 25),
            //            state = c.String(maxLength: 2),
            //            zipcode = c.String(maxLength: 10),
            //            phone = c.String(maxLength: 12),
            //            gender = c.Int(nullable: false),
            //            genderother = c.String(maxLength: 20),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "Workers",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false),
            //            typeOfWorkID = c.Int(nullable: false),
            //            dateOfMembership = c.DateTime(nullable: false),
            //            dateOfBirth = c.DateTime(nullable: false),
            //            memberStatus = c.Int(nullable: false),
            //            memberReactivateDate = c.DateTime(),
            //            active = c.Boolean(nullable: false),
            //            homeless = c.Boolean(),
            //            RaceID = c.Int(nullable: false),
            //            raceother = c.String(maxLength: 20),
            //            height = c.String(nullable: false, maxLength: 50),
            //            weight = c.String(nullable: false, maxLength: 10),
            //            englishlevelID = c.Int(nullable: false),
            //            recentarrival = c.Boolean(nullable: false),
            //            dateinUSA = c.DateTime(nullable: false),
            //            dateinseattle = c.DateTime(nullable: false),
            //            disabled = c.Boolean(nullable: false),
            //            disabilitydesc = c.String(maxLength: 50),
            //            maritalstatus = c.Int(nullable: false),
            //            livewithchildren = c.Boolean(nullable: false),
            //            numofchildren = c.Int(nullable: false),
            //            incomeID = c.Int(nullable: false),
            //            livealone = c.Boolean(nullable: false),
            //            emcontUSAname = c.String(maxLength: 50),
            //            emcontUSArelation = c.String(maxLength: 30),
            //            emcontUSAphone = c.String(maxLength: 14),
            //            dwccardnum = c.Int(nullable: false),
            //            neighborhoodID = c.Int(nullable: false),
            //            immigrantrefugee = c.Boolean(nullable: false),
            //            countryoforiginID = c.Int(nullable: false),
            //            emcontoriginname = c.String(maxLength: 50),
            //            emcontoriginrelation = c.String(maxLength: 30),
            //            emcontoriginphone = c.String(maxLength: 14),
            //            memberexpirationdate = c.DateTime(nullable: false),
            //            driverslicense = c.Boolean(nullable: false),
            //            licenseexpirationdate = c.DateTime(),
            //            carinsurance = c.Boolean(),
            //            insuranceexpiration = c.DateTime(),
            //            ImageID = c.Int(),
            //            skill1 = c.Int(),
            //            skill2 = c.Int(),
            //            skill3 = c.Int(),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Persons", t => t.ID, cascadeDelete: true)
            //    .Index(t => t.ID);
            
            //CreateTable(
            //    "WorkerSignins",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            WorkAssignmentID = c.Int(),
            //            lottery_timestamp = c.DateTime(),
            //            lottery_sequence = c.Int(),
            //            WorkerID = c.Int(),
            //            dwccardnum = c.Int(nullable: false),
            //            memberStatus = c.Int(),
            //            dateforsignin = c.DateTime(nullable: false),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Workers", t => t.WorkerID)
            //    .Index(t => t.WorkerID);
            
            //CreateTable(
            //    "WorkAssignments",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            workerAssignedID = c.Int(),
            //            workOrderID = c.Int(nullable: false),
            //            workerSigninID = c.Int(),
            //            active = c.Boolean(nullable: false),
            //            pseudoID = c.Int(),
            //            description = c.String(maxLength: 1000),
            //            englishLevelID = c.Int(nullable: false),
            //            skillID = c.Int(nullable: false),
            //            hourlyWage = c.Double(nullable: false),
            //            hours = c.Int(nullable: false),
            //            hourRange = c.Int(),
            //            days = c.Int(nullable: false),
            //            qualityOfWork = c.Int(nullable: false),
            //            followDirections = c.Int(nullable: false),
            //            attitude = c.Int(nullable: false),
            //            reliability = c.Int(nullable: false),
            //            transportProgram = c.Int(nullable: false),
            //            comments = c.String(),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("WorkOrders", t => t.workOrderID, cascadeDelete: true)
            //    .ForeignKey("WorkerSignins", t => t.workerSigninID)
            //    .ForeignKey("Workers", t => t.workerAssignedID)
            //    .Index(t => t.workOrderID)
            //    .Index(t => t.workerSigninID)
            //    .Index(t => t.workerAssignedID);
            
            //CreateTable(
            //    "WorkOrders",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            EmployerID = c.Int(nullable: false),
            //            paperOrderNum = c.Int(),
            //            waPseudoIDCounter = c.Int(nullable: false),
            //            contactName = c.String(nullable: false, maxLength: 50),
            //            status = c.Int(nullable: false),
            //            workSiteAddress1 = c.String(nullable: false, maxLength: 50),
            //            workSiteAddress2 = c.String(maxLength: 50),
            //            city = c.String(nullable: false, maxLength: 50),
            //            state = c.String(nullable: false, maxLength: 2),
            //            phone = c.String(nullable: false, maxLength: 12),
            //            zipcode = c.String(nullable: false, maxLength: 10),
            //            typeOfWorkID = c.Int(nullable: false),
            //            englishRequired = c.Boolean(nullable: false),
            //            englishRequiredNote = c.String(maxLength: 100),
            //            lunchSupplied = c.Boolean(nullable: false),
            //            permanentPlacement = c.Boolean(nullable: false),
            //            transportMethodID = c.Int(nullable: false),
            //            transportFee = c.Double(nullable: false),
            //            transportFeeExtra = c.Double(nullable: false),
            //            description = c.String(maxLength: 4000),
            //            dateTimeofWork = c.DateTime(nullable: false),
            //            timeFlexible = c.Boolean(nullable: false),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Employers", t => t.EmployerID, cascadeDelete: true)
            //    .Index(t => t.EmployerID);
            
            //CreateTable(
            //    "Employers",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            active = c.Boolean(nullable: false),
            //            business = c.Boolean(nullable: false),
            //            name = c.String(nullable: false, maxLength: 50),
            //            address1 = c.String(nullable: false, maxLength: 50),
            //            address2 = c.String(maxLength: 50),
            //            city = c.String(nullable: false, maxLength: 50),
            //            state = c.String(nullable: false, maxLength: 2),
            //            phone = c.String(nullable: false, maxLength: 12),
            //            cellphone = c.String(maxLength: 12),
            //            zipcode = c.String(nullable: false, maxLength: 10),
            //            email = c.String(maxLength: 50),
            //            referredby = c.Int(),
            //            referredbyOther = c.String(maxLength: 50),
            //            blogparticipate = c.Boolean(nullable: false),
            //            notes = c.String(maxLength: 4000),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "WorkerRequests",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            WorkOrderID = c.Int(nullable: false),
            //            WorkerID = c.Int(nullable: false),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("WorkOrders", t => t.WorkOrderID, cascadeDelete: true)
            //    .ForeignKey("Workers", t => t.WorkerID, cascadeDelete: true)
            //    .Index(t => t.WorkOrderID)
            //    .Index(t => t.WorkerID);
            
            //CreateTable(
            //    "Events",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            PersonID = c.Int(nullable: false),
            //            eventType = c.Int(nullable: false),
            //            dateFrom = c.DateTime(nullable: false),
            //            dateTo = c.DateTime(),
            //            notes = c.String(maxLength: 4000),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Persons", t => t.PersonID, cascadeDelete: true)
            //    .Index(t => t.PersonID);
            
            //CreateTable(
            //    "JoinEventImages",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            EventID = c.Int(nullable: false),
            //            ImageID = c.Int(nullable: false),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Events", t => t.EventID, cascadeDelete: true)
            //    .ForeignKey("Images", t => t.ImageID, cascadeDelete: true)
            //    .Index(t => t.EventID)
            //    .Index(t => t.ImageID);
            
            //CreateTable(
            //    "Images",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            ImageData = c.Binary(),
            //            ImageMimeType = c.String(maxLength: 30),
            //            filename = c.String(maxLength: 255),
            //            Thumbnail = c.Binary(),
            //            ThumbnailMimeType = c.String(maxLength: 30),
            //            parenttable = c.String(maxLength: 30),
            //            recordkey = c.String(maxLength: 20),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "Lookups",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            category = c.String(nullable: false, maxLength: 20),
            //            text_EN = c.String(nullable: false, maxLength: 50),
            //            text_ES = c.String(nullable: false, maxLength: 50),
            //            selected = c.Boolean(nullable: false),
            //            subcategory = c.String(maxLength: 20),
            //            level = c.Int(),
            //            wage = c.Double(),
            //            minHour = c.Int(),
            //            fixedJob = c.Boolean(),
            //            sortorder = c.Int(),
            //            typeOfWorkID = c.Int(),
            //            speciality = c.Boolean(nullable: false),
            //            ltrCode = c.String(maxLength: 1),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "Activities",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            name = c.Int(nullable: false),
            //            type = c.Int(nullable: false),
            //            dateStart = c.DateTime(nullable: false),
            //            dateEnd = c.DateTime(nullable: false),
            //            teacher = c.String(nullable: false),
            //            notes = c.String(maxLength: 4000),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "ActivitySignins",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            activityID = c.Int(nullable: false),
            //            personID = c.Int(),
            //            dwccardnum = c.Int(nullable: false),
            //            memberStatus = c.Int(),
            //            dateforsignin = c.DateTime(nullable: false),
            //            datecreated = c.DateTime(nullable: false),
            //            dateupdated = c.DateTime(nullable: false),
            //            Createdby = c.String(maxLength: 30),
            //            Updatedby = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("Persons", t => t.personID)
            //    .ForeignKey("Activities", t => t.activityID, cascadeDelete: true)
            //    .Index(t => t.personID)
            //    .Index(t => t.activityID);
            
        }
        
        public override void Down()
        {
            //DropIndex("ActivitySignins", new[] { "activityID" });
            //DropIndex("ActivitySignins", new[] { "personID" });
            //DropIndex("JoinEventImages", new[] { "ImageID" });
            //DropIndex("JoinEventImages", new[] { "EventID" });
            //DropIndex("Events", new[] { "PersonID" });
            //DropIndex("WorkerRequests", new[] { "WorkerID" });
            //DropIndex("WorkerRequests", new[] { "WorkOrderID" });
            //DropIndex("WorkOrders", new[] { "EmployerID" });
            //DropIndex("WorkAssignments", new[] { "workerAssignedID" });
            //DropIndex("WorkAssignments", new[] { "workerSigninID" });
            //DropIndex("WorkAssignments", new[] { "workOrderID" });
            //DropIndex("WorkerSignins", new[] { "WorkerID" });
            //DropIndex("Workers", new[] { "ID" });
            //DropForeignKey("ActivitySignins", "activityID", "Activities");
            //DropForeignKey("ActivitySignins", "personID", "Persons");
            //DropForeignKey("JoinEventImages", "ImageID", "Images");
            //DropForeignKey("JoinEventImages", "EventID", "Events");
            //DropForeignKey("Events", "PersonID", "Persons");
            //DropForeignKey("WorkerRequests", "WorkerID", "Workers");
            //DropForeignKey("WorkerRequests", "WorkOrderID", "WorkOrders");
            //DropForeignKey("WorkOrders", "EmployerID", "Employers");
            //DropForeignKey("WorkAssignments", "workerAssignedID", "Workers");
            //DropForeignKey("WorkAssignments", "workerSigninID", "WorkerSignins");
            //DropForeignKey("WorkAssignments", "workOrderID", "WorkOrders");
            //DropForeignKey("WorkerSignins", "WorkerID", "Workers");
            //DropForeignKey("Workers", "ID", "Persons");
            //DropTable("ActivitySignins");
            //DropTable("Activities");
            //DropTable("Lookups");
            //DropTable("Images");
            //DropTable("JoinEventImages");
            //DropTable("Events");
            //DropTable("WorkerRequests");
            //DropTable("Employers");
            //DropTable("WorkOrders");
            //DropTable("WorkAssignments");
            //DropTable("WorkerSignins");
            //DropTable("Workers");
            //DropTable("Persons");
        }
    }
}
