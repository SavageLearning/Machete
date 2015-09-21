namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        dateStart = c.DateTime(nullable: false),
                        dateEnd = c.DateTime(nullable: false),
                        recurring = c.Boolean(nullable: false),
                        firstID = c.Int(nullable: false),
                        teacher = c.String(nullable: false),
                        notes = c.String(maxLength: 4000),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ActivitySignins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        activityID = c.Int(nullable: false),
                        personID = c.Int(),
                        dwccardnum = c.Int(nullable: false),
                        memberStatus = c.Int(),
                        dateforsignin = c.DateTime(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Persons", t => t.personID)
                .ForeignKey("dbo.Activities", t => t.activityID, cascadeDelete: true)
                .Index(t => t.personID)
                .Index(t => t.activityID);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        active = c.Boolean(nullable: false),
                        firstname1 = c.String(nullable: false, maxLength: 50),
                        firstname2 = c.String(maxLength: 50),
                        lastname1 = c.String(nullable: false, maxLength: 50),
                        lastname2 = c.String(maxLength: 50),
                        address1 = c.String(maxLength: 50),
                        address2 = c.String(maxLength: 50),
                        city = c.String(maxLength: 25),
                        state = c.String(maxLength: 2),
                        zipcode = c.String(maxLength: 10),
                        phone = c.String(maxLength: 12),
                        gender = c.Int(nullable: false),
                        genderother = c.String(maxLength: 20),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PersonID = c.Int(nullable: false),
                        eventType = c.Int(nullable: false),
                        dateFrom = c.DateTime(nullable: false),
                        dateTo = c.DateTime(),
                        notes = c.String(maxLength: 4000),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Persons", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.JoinEventImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EventID = c.Int(nullable: false),
                        ImageID = c.Int(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ImageID, cascadeDelete: true)
                .Index(t => t.EventID)
                .Index(t => t.ImageID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(maxLength: 30),
                        filename = c.String(maxLength: 255),
                        Thumbnail = c.Binary(),
                        ThumbnailMimeType = c.String(maxLength: 30),
                        parenttable = c.String(maxLength: 30),
                        recordkey = c.String(maxLength: 20),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        typeOfWorkID = c.Int(nullable: false),
                        dateOfMembership = c.DateTime(nullable: false),
                        dateOfBirth = c.DateTime(nullable: false),
                        memberStatus = c.Int(nullable: false),
                        memberReactivateDate = c.DateTime(),
                        active = c.Boolean(nullable: false),
                        homeless = c.Boolean(),
                        RaceID = c.Int(nullable: false),
                        raceother = c.String(maxLength: 20),
                        height = c.String(nullable: false, maxLength: 50),
                        weight = c.String(nullable: false, maxLength: 10),
                        englishlevelID = c.Int(nullable: false),
                        recentarrival = c.Boolean(nullable: false),
                        dateinUSA = c.DateTime(nullable: false),
                        dateinseattle = c.DateTime(nullable: false),
                        disabled = c.Boolean(nullable: false),
                        disabilitydesc = c.String(maxLength: 50),
                        maritalstatus = c.Int(nullable: false),
                        livewithchildren = c.Boolean(nullable: false),
                        numofchildren = c.Int(nullable: false),
                        incomeID = c.Int(nullable: false),
                        livealone = c.Boolean(nullable: false),
                        emcontUSAname = c.String(maxLength: 50),
                        emcontUSArelation = c.String(maxLength: 30),
                        emcontUSAphone = c.String(maxLength: 14),
                        dwccardnum = c.Int(nullable: false),
                        neighborhoodID = c.Int(nullable: false),
                        immigrantrefugee = c.Boolean(nullable: false),
                        countryoforiginID = c.Int(nullable: false),
                        emcontoriginname = c.String(maxLength: 50),
                        emcontoriginrelation = c.String(maxLength: 30),
                        emcontoriginphone = c.String(maxLength: 14),
                        memberexpirationdate = c.DateTime(nullable: false),
                        driverslicense = c.Boolean(nullable: false),
                        licenseexpirationdate = c.DateTime(),
                        carinsurance = c.Boolean(),
                        insuranceexpiration = c.DateTime(),
                        ImageID = c.Int(),
                        skill1 = c.Int(),
                        skill2 = c.Int(),
                        skill3 = c.Int(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Persons", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.WorkAssignments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        workerAssignedID = c.Int(),
                        workOrderID = c.Int(nullable: false),
                        workerSigninID = c.Int(),
                        active = c.Boolean(nullable: false),
                        pseudoID = c.Int(),
                        description = c.String(maxLength: 1000),
                        englishLevelID = c.Int(nullable: false),
                        skillID = c.Int(nullable: false),
                        surcharge = c.Double(nullable: false),
                        hourlyWage = c.Double(nullable: false),
                        hours = c.Int(nullable: false),
                        hourRange = c.Int(),
                        days = c.Int(nullable: false),
                        qualityOfWork = c.Int(nullable: false),
                        followDirections = c.Int(nullable: false),
                        attitude = c.Int(nullable: false),
                        reliability = c.Int(nullable: false),
                        transportProgram = c.Int(nullable: false),
                        comments = c.String(),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkerSignins", t => t.workerSigninID)
                .ForeignKey("dbo.WorkOrders", t => t.workOrderID, cascadeDelete: true)
                .ForeignKey("dbo.Workers", t => t.workerAssignedID)
                .Index(t => t.workerSigninID)
                .Index(t => t.workOrderID)
                .Index(t => t.workerAssignedID);
            
            CreateTable(
                "dbo.WorkerSignins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkAssignmentID = c.Int(),
                        lottery_timestamp = c.DateTime(),
                        lottery_sequence = c.Int(),
                        WorkerID = c.Int(),
                        dwccardnum = c.Int(nullable: false),
                        memberStatus = c.Int(),
                        dateforsignin = c.DateTime(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workers", t => t.WorkerID)
                .Index(t => t.WorkerID);
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployerID = c.Int(nullable: false),
                        onlineSource = c.Boolean(nullable: false),
                        paperOrderNum = c.Int(),
                        waPseudoIDCounter = c.Int(nullable: false),
                        contactName = c.String(nullable: false, maxLength: 50),
                        status = c.Int(nullable: false),
                        workSiteAddress1 = c.String(nullable: false, maxLength: 50),
                        workSiteAddress2 = c.String(maxLength: 50),
                        city = c.String(nullable: false, maxLength: 50),
                        state = c.String(nullable: false, maxLength: 2),
                        phone = c.String(nullable: false, maxLength: 12),
                        zipcode = c.String(nullable: false, maxLength: 10),
                        typeOfWorkID = c.Int(nullable: false),
                        englishRequired = c.Boolean(nullable: false),
                        englishRequiredNote = c.String(maxLength: 100),
                        lunchSupplied = c.Boolean(nullable: false),
                        permanentPlacement = c.Boolean(nullable: false),
                        transportMethodID = c.Int(nullable: false),
                        transportFee = c.Double(nullable: false),
                        transportFeeExtra = c.Double(nullable: false),
                        description = c.String(maxLength: 4000),
                        dateTimeofWork = c.DateTime(nullable: false),
                        timeFlexible = c.Boolean(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employers", t => t.EmployerID, cascadeDelete: true)
                .Index(t => t.EmployerID);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        emailFrom = c.String(maxLength: 50),
                        emailTo = c.String(nullable: false, maxLength: 50),
                        subject = c.String(nullable: false, maxLength: 100),
                        body = c.String(nullable: false),
                        transmitAttempts = c.Int(nullable: false),
                        statusID = c.Int(nullable: false),
                        lastAttempt = c.DateTime(),
                        attachment = c.String(),
                        attachmentContentType = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Employers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        active = c.Boolean(nullable: false),
                        onlineSource = c.Boolean(nullable: false),
                        returnCustomer = c.Boolean(nullable: false),
                        receiveUpdates = c.Boolean(nullable: false),
                        business = c.Boolean(nullable: false),
                        name = c.String(nullable: false, maxLength: 50),
                        address1 = c.String(nullable: false, maxLength: 50),
                        address2 = c.String(maxLength: 50),
                        city = c.String(nullable: false, maxLength: 50),
                        state = c.String(nullable: false, maxLength: 2),
                        phone = c.String(nullable: false, maxLength: 12),
                        cellphone = c.String(maxLength: 12),
                        zipcode = c.String(nullable: false, maxLength: 10),
                        email = c.String(maxLength: 50),
                        referredby = c.Int(),
                        referredbyOther = c.String(maxLength: 50),
                        blogparticipate = c.Boolean(nullable: false),
                        notes = c.String(maxLength: 4000),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.WorkerRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkOrderID = c.Int(nullable: false),
                        WorkerID = c.Int(nullable: false),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workers", t => t.WorkerID, cascadeDelete: true)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderID, cascadeDelete: true)
                .Index(t => t.WorkerID)
                .Index(t => t.WorkOrderID);
            
            CreateTable(
                "dbo.Lookups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        category = c.String(nullable: false, maxLength: 20),
                        text_EN = c.String(nullable: false, maxLength: 50),
                        text_ES = c.String(nullable: false, maxLength: 50),
                        selected = c.Boolean(nullable: false),
                        subcategory = c.String(maxLength: 20),
                        level = c.Int(),
                        wage = c.Double(),
                        minHour = c.Int(),
                        fixedJob = c.Boolean(),
                        sortorder = c.Int(),
                        typeOfWorkID = c.Int(),
                        speciality = c.Boolean(nullable: false),
                        ltrCode = c.String(maxLength: 3),
                        emailTemplate = c.String(),
                        key = c.String(maxLength: 30),
                        datecreated = c.DateTime(nullable: false),
                        dateupdated = c.DateTime(nullable: false),
                        Createdby = c.String(maxLength: 30),
                        Updatedby = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        ApplicationId = c.Guid(),
                        MobileAlias = c.String(),
                        IsAnonymous = c.Boolean(),
                        LastActivityDate = c.DateTime(),
                        MobilePIN = c.String(),
                        Email = c.String(),
                        LoweredEmail = c.String(),
                        LoweredUserName = c.String(),
                        PasswordQuestion = c.String(),
                        PasswordAnswer = c.String(),
                        IsApproved = c.Boolean(),
                        IsLockedOut = c.Boolean(),
                        CreateDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        LastPasswordChangedDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        FailedPasswordAttemptCount = c.Int(),
                        FailedPasswordAttemptWindowStart = c.DateTime(),
                        FailedPasswordAnswerAttemptCount = c.Int(),
                        FailedPasswordAnswerAttemptWindowStart = c.DateTime(),
                        Comment = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmailWorkOrders",
                c => new
                    {
                        Email_ID = c.Int(nullable: false),
                        WorkOrder_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Email_ID, t.WorkOrder_ID })
                .ForeignKey("dbo.Emails", t => t.Email_ID, cascadeDelete: true)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrder_ID, cascadeDelete: true)
                .Index(t => t.Email_ID)
                .Index(t => t.WorkOrder_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ActivitySignins", "activityID", "dbo.Activities");
            DropForeignKey("dbo.ActivitySignins", "personID", "dbo.Persons");
            DropForeignKey("dbo.Workers", "ID", "dbo.Persons");
            DropForeignKey("dbo.WorkerSignins", "WorkerID", "dbo.Workers");
            DropForeignKey("dbo.WorkAssignments", "workerAssignedID", "dbo.Workers");
            DropForeignKey("dbo.WorkerRequests", "WorkOrderID", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkerRequests", "WorkerID", "dbo.Workers");
            DropForeignKey("dbo.WorkAssignments", "workOrderID", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "EmployerID", "dbo.Employers");
            DropForeignKey("dbo.EmailWorkOrders", "WorkOrder_ID", "dbo.WorkOrders");
            DropForeignKey("dbo.EmailWorkOrders", "Email_ID", "dbo.Emails");
            DropForeignKey("dbo.WorkAssignments", "workerSigninID", "dbo.WorkerSignins");
            DropForeignKey("dbo.Events", "PersonID", "dbo.Persons");
            DropForeignKey("dbo.JoinEventImages", "ImageID", "dbo.Images");
            DropForeignKey("dbo.JoinEventImages", "EventID", "dbo.Events");
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ActivitySignins", new[] { "activityID" });
            DropIndex("dbo.ActivitySignins", new[] { "personID" });
            DropIndex("dbo.Workers", new[] { "ID" });
            DropIndex("dbo.WorkerSignins", new[] { "WorkerID" });
            DropIndex("dbo.WorkAssignments", new[] { "workerAssignedID" });
            DropIndex("dbo.WorkerRequests", new[] { "WorkOrderID" });
            DropIndex("dbo.WorkerRequests", new[] { "WorkerID" });
            DropIndex("dbo.WorkAssignments", new[] { "workOrderID" });
            DropIndex("dbo.WorkOrders", new[] { "EmployerID" });
            DropIndex("dbo.EmailWorkOrders", new[] { "WorkOrder_ID" });
            DropIndex("dbo.EmailWorkOrders", new[] { "Email_ID" });
            DropIndex("dbo.WorkAssignments", new[] { "workerSigninID" });
            DropIndex("dbo.Events", new[] { "PersonID" });
            DropIndex("dbo.JoinEventImages", new[] { "ImageID" });
            DropIndex("dbo.JoinEventImages", new[] { "EventID" });
            DropTable("dbo.EmailWorkOrders");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Lookups");
            DropTable("dbo.WorkerRequests");
            DropTable("dbo.Employers");
            DropTable("dbo.Emails");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.WorkerSignins");
            DropTable("dbo.WorkAssignments");
            DropTable("dbo.Workers");
            DropTable("dbo.Images");
            DropTable("dbo.JoinEventImages");
            DropTable("dbo.Events");
            DropTable("dbo.Persons");
            DropTable("dbo.ActivitySignins");
            DropTable("dbo.Activities");
        }
    }
}
