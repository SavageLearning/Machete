using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Machete.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    name = table.Column<int>(nullable: false),
                    nameEN = table.Column<string>(maxLength: 50, nullable: true),
                    nameES = table.Column<string>(maxLength: 50, nullable: true),
                    type = table.Column<int>(nullable: false),
                    typeEN = table.Column<string>(maxLength: 50, nullable: true),
                    typeES = table.Column<string>(maxLength: 50, nullable: true),
                    dateStart = table.Column<DateTime>(nullable: false),
                    dateEnd = table.Column<DateTime>(nullable: false),
                    recurring = table.Column<bool>(nullable: false),
                    firstID = table.Column<int>(nullable: false),
                    teacher = table.Column<string>(nullable: false),
                    notes = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    MobileAlias = table.Column<string>(nullable: true),
                    IsAnonymous = table.Column<bool>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(nullable: false),
                    MobilePIN = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    LoweredEmail = table.Column<string>(nullable: true),
                    LoweredUserName = table.Column<string>(nullable: true),
                    PasswordQuestion = table.Column<string>(nullable: true),
                    PasswordAnswer = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsLockedOut = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastLockoutDate = table.Column<DateTime>(nullable: false),
                    FailedPasswordAttemptCount = table.Column<int>(nullable: false),
                    FailedPasswordAttemptWindowStart = table.Column<DateTime>(nullable: false),
                    FailedPasswordAnswerAttemptCount = table.Column<int>(nullable: false),
                    FailedPasswordAnswerAttemptWindowStart = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    key = table.Column<string>(maxLength: 50, nullable: false),
                    value = table.Column<string>(maxLength: 5000, nullable: false),
                    description = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    publicConfig = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    emailFrom = table.Column<string>(maxLength: 50, nullable: true),
                    emailTo = table.Column<string>(maxLength: 50, nullable: false),
                    subject = table.Column<string>(maxLength: 100, nullable: false),
                    body = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    transmitAttempts = table.Column<int>(nullable: false),
                    statusID = table.Column<int>(nullable: false),
                    lastAttempt = table.Column<DateTime>(nullable: true),
                    attachment = table.Column<string>(nullable: true),
                    attachmentContentType = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    active = table.Column<bool>(nullable: false),
                    onlineSource = table.Column<bool>(nullable: false),
                    returnCustomer = table.Column<bool>(nullable: false),
                    receiveUpdates = table.Column<bool>(nullable: false),
                    business = table.Column<bool>(nullable: false),
                    businessname = table.Column<string>(nullable: true),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    address1 = table.Column<string>(maxLength: 50, nullable: false),
                    address2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: false),
                    state = table.Column<string>(maxLength: 2, nullable: false),
                    phone = table.Column<string>(maxLength: 12, nullable: false),
                    fax = table.Column<string>(maxLength: 12, nullable: true),
                    cellphone = table.Column<string>(maxLength: 12, nullable: true),
                    zipcode = table.Column<string>(maxLength: 10, nullable: false),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    licenseplate = table.Column<string>(maxLength: 10, nullable: true),
                    driverslicense = table.Column<string>(maxLength: 30, nullable: true),
                    referredby = table.Column<int>(nullable: true),
                    referredbyOther = table.Column<string>(maxLength: 50, nullable: true),
                    blogparticipate = table.Column<bool>(nullable: true),
                    notes = table.Column<string>(maxLength: 4000, nullable: true),
                    onlineSigninID = table.Column<string>(maxLength: 128, nullable: true),
                    isOnlineProfileComplete = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(maxLength: 30, nullable: true),
                    filename = table.Column<string>(maxLength: 255, nullable: true),
                    Thumbnail = table.Column<byte[]>(nullable: true),
                    ThumbnailMimeType = table.Column<string>(maxLength: 30, nullable: true),
                    parenttable = table.Column<string>(maxLength: 30, nullable: true),
                    recordkey = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    category = table.Column<string>(maxLength: 20, nullable: false),
                    text_EN = table.Column<string>(maxLength: 50, nullable: false),
                    text_ES = table.Column<string>(maxLength: 50, nullable: false),
                    selected = table.Column<bool>(nullable: false),
                    subcategory = table.Column<string>(maxLength: 20, nullable: true),
                    level = table.Column<int>(nullable: true),
                    wage = table.Column<double>(nullable: true),
                    minHour = table.Column<int>(nullable: true),
                    fixedJob = table.Column<bool>(nullable: true),
                    sortorder = table.Column<int>(nullable: true),
                    typeOfWorkID = table.Column<int>(nullable: true),
                    speciality = table.Column<bool>(nullable: false),
                    ltrCode = table.Column<string>(maxLength: 3, nullable: true),
                    emailTemplate = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    skillDescriptionEn = table.Column<string>(maxLength: 300, nullable: true),
                    skillDescriptionEs = table.Column<string>(maxLength: 300, nullable: true),
                    minimumCost = table.Column<double>(nullable: true),
                    key = table.Column<string>(maxLength: 30, nullable: true),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    active = table.Column<bool>(nullable: false),
                    firstname1 = table.Column<string>(maxLength: 50, nullable: false),
                    firstname2 = table.Column<string>(maxLength: 50, nullable: true),
                    nickname = table.Column<string>(maxLength: 50, nullable: true),
                    lastname1 = table.Column<string>(maxLength: 50, nullable: false),
                    lastname2 = table.Column<string>(maxLength: 50, nullable: true),
                    address1 = table.Column<string>(maxLength: 50, nullable: true),
                    address2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 25, nullable: true),
                    state = table.Column<string>(maxLength: 2, nullable: true),
                    zipcode = table.Column<string>(maxLength: 10, nullable: true),
                    phone = table.Column<string>(maxLength: 12, nullable: true),
                    cellphone = table.Column<string>(maxLength: 12, nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    facebook = table.Column<string>(maxLength: 50, nullable: true),
                    gender = table.Column<int>(nullable: false),
                    genderother = table.Column<string>(maxLength: 20, nullable: true),
                    fullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReportDefinitions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    name = table.Column<string>(nullable: true),
                    commonName = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    sqlquery = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    subcategory = table.Column<string>(nullable: true),
                    inputsJson = table.Column<string>(nullable: true),
                    columnsJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleRules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    day = table.Column<int>(nullable: false),
                    leadHours = table.Column<int>(nullable: false),
                    minStartMin = table.Column<int>(nullable: false),
                    maxEndMin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransportProviders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    text_EN = table.Column<string>(maxLength: 50, nullable: true),
                    text_ES = table.Column<string>(maxLength: 50, nullable: true),
                    defaultAttribute = table.Column<bool>(nullable: false),
                    sortorder = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportProviders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransportRules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    lookupKey = table.Column<string>(maxLength: 50, nullable: true),
                    zoneLabel = table.Column<string>(maxLength: 50, nullable: true),
                    zipcodes = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    EmployerID = table.Column<int>(nullable: false),
                    timeZoneOffset = table.Column<double>(nullable: false),
                    onlineSource = table.Column<bool>(nullable: false),
                    paperOrderNum = table.Column<int>(nullable: true),
                    waPseudoIDCounter = table.Column<int>(nullable: false),
                    contactName = table.Column<string>(maxLength: 50, nullable: false),
                    status = table.Column<int>(nullable: false),
                    statusEN = table.Column<string>(maxLength: 50, nullable: true),
                    statusES = table.Column<string>(maxLength: 50, nullable: true),
                    workSiteAddress1 = table.Column<string>(maxLength: 50, nullable: false),
                    workSiteAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: false),
                    state = table.Column<string>(maxLength: 2, nullable: false),
                    zipcode = table.Column<string>(maxLength: 10, nullable: false),
                    phone = table.Column<string>(maxLength: 12, nullable: false),
                    typeOfWorkID = table.Column<int>(nullable: false),
                    englishRequired = table.Column<bool>(nullable: false),
                    englishRequiredNote = table.Column<string>(maxLength: 100, nullable: true),
                    lunchSupplied = table.Column<bool>(nullable: false),
                    permanentPlacement = table.Column<bool>(nullable: false),
                    transportMethodID = table.Column<int>(nullable: false),
                    transportMethodEN = table.Column<string>(nullable: true),
                    transportMethodES = table.Column<string>(nullable: true),
                    transportProviderID = table.Column<int>(nullable: false),
                    transportFee = table.Column<double>(nullable: false),
                    transportFeeExtra = table.Column<double>(nullable: false),
                    transportTransactType = table.Column<int>(nullable: true),
                    transportTransactID = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    dateTimeofWork = table.Column<DateTime>(nullable: false),
                    timeFlexible = table.Column<bool>(nullable: false),
                    additionalNotes = table.Column<string>(maxLength: 1000, nullable: true),
                    disclosureAgreement = table.Column<bool>(nullable: true),
                    ppFee = table.Column<double>(nullable: true),
                    ppResponse = table.Column<string>(maxLength: 5000, nullable: true),
                    ppPaymentToken = table.Column<string>(maxLength: 25, nullable: true),
                    ppPaymentID = table.Column<string>(maxLength: 50, nullable: true),
                    ppPayerID = table.Column<string>(maxLength: 25, nullable: true),
                    ppState = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Employers_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "Employers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySignins",
                columns: table => new
                {
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    dwccardnum = table.Column<int>(nullable: false),
                    memberStatus = table.Column<int>(nullable: true),
                    dateforsignin = table.Column<DateTime>(nullable: false),
                    timeZoneOffset = table.Column<double>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    activityID = table.Column<int>(nullable: false),
                    personID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySignins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActivitySignins_Activities_activityID",
                        column: x => x.activityID,
                        principalTable: "Activities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivitySignins_Persons_personID",
                        column: x => x.personID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    PersonID = table.Column<int>(nullable: false),
                    eventType = table.Column<int>(nullable: false),
                    eventTypeEN = table.Column<string>(maxLength: 50, nullable: true),
                    eventTypeES = table.Column<string>(maxLength: 50, nullable: true),
                    dateFrom = table.Column<DateTime>(nullable: false),
                    dateTo = table.Column<DateTime>(nullable: true),
                    notes = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Events_Persons_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    fullNameAndID = table.Column<string>(maxLength: 100, nullable: true),
                    typeOfWorkID = table.Column<int>(nullable: false),
                    typeOfWork = table.Column<string>(nullable: true),
                    dateOfMembership = table.Column<DateTime>(nullable: false),
                    dateOfBirth = table.Column<DateTime>(nullable: true),
                    memberStatus = table.Column<int>(nullable: false),
                    memberStatusEN = table.Column<string>(maxLength: 50, nullable: true),
                    memberStatusES = table.Column<string>(maxLength: 50, nullable: true),
                    memberReactivateDate = table.Column<DateTime>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    homeless = table.Column<bool>(nullable: true),
                    housingType = table.Column<int>(nullable: true),
                    RaceID = table.Column<int>(nullable: true),
                    raceother = table.Column<string>(maxLength: 20, nullable: true),
                    height = table.Column<string>(maxLength: 50, nullable: true),
                    weight = table.Column<string>(maxLength: 10, nullable: true),
                    englishlevelID = table.Column<int>(nullable: false),
                    recentarrival = table.Column<bool>(nullable: true),
                    dateinUSA = table.Column<DateTime>(nullable: true),
                    dateinseattle = table.Column<DateTime>(nullable: true),
                    disabled = table.Column<bool>(nullable: true),
                    disabilitydesc = table.Column<string>(maxLength: 50, nullable: true),
                    maritalstatus = table.Column<int>(nullable: true),
                    livewithchildren = table.Column<bool>(nullable: true),
                    liveWithSpouse = table.Column<bool>(nullable: true),
                    livealone = table.Column<bool>(nullable: true),
                    liveWithDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    numofchildren = table.Column<int>(nullable: true),
                    americanBornChildren = table.Column<int>(nullable: true),
                    numChildrenUnder18 = table.Column<int>(nullable: true),
                    educationLevel = table.Column<int>(nullable: true),
                    farmLaborCharacteristics = table.Column<int>(nullable: true),
                    wageTheftVictim = table.Column<bool>(nullable: true),
                    wageTheftRecoveryAmount = table.Column<double>(nullable: true),
                    incomeID = table.Column<int>(nullable: true),
                    emcontUSAname = table.Column<string>(maxLength: 50, nullable: true),
                    emcontUSArelation = table.Column<string>(maxLength: 30, nullable: true),
                    emcontUSAphone = table.Column<string>(maxLength: 14, nullable: true),
                    dwccardnum = table.Column<int>(nullable: false),
                    neighborhoodID = table.Column<int>(nullable: true),
                    immigrantrefugee = table.Column<bool>(nullable: true),
                    countryoforiginID = table.Column<int>(nullable: true),
                    emcontoriginname = table.Column<string>(maxLength: 50, nullable: true),
                    emcontoriginrelation = table.Column<string>(maxLength: 30, nullable: true),
                    emcontoriginphone = table.Column<string>(maxLength: 14, nullable: true),
                    memberexpirationdate = table.Column<DateTime>(nullable: false),
                    driverslicense = table.Column<bool>(nullable: true),
                    licenseexpirationdate = table.Column<DateTime>(nullable: true),
                    carinsurance = table.Column<bool>(nullable: true),
                    insuranceexpiration = table.Column<DateTime>(nullable: true),
                    lastPaymentDate = table.Column<DateTime>(nullable: true),
                    lastPaymentAmount = table.Column<double>(nullable: true),
                    ownTools = table.Column<bool>(nullable: true),
                    healthInsurance = table.Column<bool>(nullable: true),
                    usVeteran = table.Column<bool>(nullable: true),
                    healthInsuranceDate = table.Column<DateTime>(nullable: true),
                    vehicleTypeID = table.Column<int>(nullable: true),
                    incomeSourceID = table.Column<int>(nullable: true),
                    introToCenter = table.Column<string>(maxLength: 1000, nullable: true),
                    ImageID = table.Column<int>(nullable: true),
                    skill1 = table.Column<int>(nullable: true),
                    skill2 = table.Column<int>(nullable: true),
                    skill3 = table.Column<int>(nullable: true),
                    skillCodes = table.Column<string>(nullable: true),
                    workerRating = table.Column<float>(nullable: true),
                    lgbtq = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Workers_Persons_ID",
                        column: x => x.ID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportProvidersAvailability",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    transportProviderID = table.Column<int>(nullable: false),
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    lookupKey = table.Column<string>(maxLength: 50, nullable: true),
                    day = table.Column<int>(nullable: false),
                    available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportProvidersAvailability", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransportProvidersAvailability_TransportProviders_transportProviderID",
                        column: x => x.transportProviderID,
                        principalTable: "TransportProviders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCostRules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    transportRuleID = table.Column<int>(nullable: false),
                    minWorker = table.Column<int>(nullable: false),
                    maxWorker = table.Column<int>(nullable: false),
                    cost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCostRules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransportCostRules_TransportRules_transportRuleID",
                        column: x => x.transportRuleID,
                        principalTable: "TransportRules",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinWorkOrderEmail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    WorkOrderID = table.Column<int>(nullable: false),
                    EmailID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinWorkOrderEmail", x => new { x.EmailID, x.WorkOrderID });
                    table.ForeignKey(
                        name: "FK_JoinWorkOrderEmail_Emails_EmailID",
                        column: x => x.EmailID,
                        principalTable: "Emails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinWorkOrderEmail_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinEventImage",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    EventID = table.Column<int>(nullable: false),
                    ImageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinEventImage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JoinEventImage_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinEventImage_Images_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Images",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerRequests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    WorkOrderID = table.Column<int>(nullable: false),
                    WorkerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkerRequests_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerRequests_Workers_WorkerID",
                        column: x => x.WorkerID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerSignins",
                columns: table => new
                {
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    dwccardnum = table.Column<int>(nullable: false),
                    memberStatus = table.Column<int>(nullable: true),
                    dateforsignin = table.Column<DateTime>(nullable: false),
                    timeZoneOffset = table.Column<double>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkAssignmentID = table.Column<int>(nullable: true),
                    lottery_timestamp = table.Column<DateTime>(nullable: true),
                    lottery_sequence = table.Column<int>(nullable: true),
                    WorkerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerSignins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkerSignins_Workers_WorkerID",
                        column: x => x.WorkerID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkAssignments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    workerAssignedID = table.Column<int>(nullable: true),
                    workOrderID = table.Column<int>(nullable: false),
                    workerSigninID = table.Column<int>(nullable: true),
                    workerSigininID = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    pseudoID = table.Column<int>(nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    englishLevelID = table.Column<int>(nullable: false),
                    skillID = table.Column<int>(nullable: false),
                    skillEN = table.Column<string>(nullable: true),
                    skillES = table.Column<string>(nullable: true),
                    surcharge = table.Column<double>(nullable: false),
                    hourlyWage = table.Column<double>(nullable: false),
                    hours = table.Column<double>(nullable: false),
                    hourRange = table.Column<int>(nullable: true),
                    days = table.Column<int>(nullable: false),
                    qualityOfWork = table.Column<int>(nullable: false),
                    followDirections = table.Column<int>(nullable: false),
                    attitude = table.Column<int>(nullable: false),
                    reliability = table.Column<int>(nullable: false),
                    transportProgram = table.Column<int>(nullable: false),
                    comments = table.Column<string>(nullable: true),
                    workerRating = table.Column<int>(nullable: true),
                    workerRatingComments = table.Column<string>(maxLength: 500, nullable: true),
                    weightLifted = table.Column<bool>(nullable: true),
                    fullWAID = table.Column<string>(nullable: true),
                    minEarnings = table.Column<double>(nullable: false),
                    maxEarnings = table.Column<double>(nullable: false),
                    transportCost = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAssignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkAssignments_WorkOrders_workOrderID",
                        column: x => x.workOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkAssignments_Workers_workerAssignedID",
                        column: x => x.workerAssignedID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkAssignments_WorkerSignins_workerSigininID",
                        column: x => x.workerSigininID,
                        principalTable: "WorkerSignins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySignins_activityID",
                table: "ActivitySignins",
                column: "activityID");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySignins_personID",
                table: "ActivitySignins",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PersonID",
                table: "Events",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_JoinEventImage_EventID",
                table: "JoinEventImage",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_JoinEventImage_ImageID",
                table: "JoinEventImage",
                column: "ImageID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinWorkOrderEmail_WorkOrderID",
                table: "JoinWorkOrderEmail",
                column: "WorkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostRules_transportRuleID",
                table: "TransportCostRules",
                column: "transportRuleID");

            migrationBuilder.CreateIndex(
                name: "IX_TransportProvidersAvailability_transportProviderID",
                table: "TransportProvidersAvailability",
                column: "transportProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAssignments_workOrderID",
                table: "WorkAssignments",
                column: "workOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAssignments_workerAssignedID",
                table: "WorkAssignments",
                column: "workerAssignedID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAssignments_workerSigininID",
                table: "WorkAssignments",
                column: "workerSigininID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRequests_WorkOrderID",
                table: "WorkerRequests",
                column: "WorkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRequests_WorkerID",
                table: "WorkerRequests",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSignins_WorkerID",
                table: "WorkerSignins",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_EmployerID",
                table: "WorkOrders",
                column: "EmployerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitySignins");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "JoinEventImage");

            migrationBuilder.DropTable(
                name: "JoinWorkOrderEmail");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "ReportDefinitions");

            migrationBuilder.DropTable(
                name: "ScheduleRules");

            migrationBuilder.DropTable(
                name: "TransportCostRules");

            migrationBuilder.DropTable(
                name: "TransportProvidersAvailability");

            migrationBuilder.DropTable(
                name: "WorkAssignments");

            migrationBuilder.DropTable(
                name: "WorkerRequests");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "TransportRules");

            migrationBuilder.DropTable(
                name: "TransportProviders");

            migrationBuilder.DropTable(
                name: "WorkerSignins");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
