using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Machete.Data.Migrations
{
    public partial class Prehistoric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__MigrationHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(maxLength: 150, nullable: false),
                    ContextKey = table.Column<string>(maxLength: 300, nullable: false),
                    Model = table.Column<byte[]>(nullable: false),
                    ProductVersion = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.__MigrationHistory", x => new { x.MigrationId, x.ContextKey });
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    dateStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    teacher = table.Column<string>(nullable: false),
                    notes = table.Column<string>(maxLength: 4000, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    recurring = table.Column<bool>(nullable: false),
                    firstID = table.Column<int>(nullable: false),
                    nameEN = table.Column<string>(maxLength: 50, nullable: true),
                    nameES = table.Column<string>(maxLength: 50, nullable: true),
                    typeEN = table.Column<string>(maxLength: 50, nullable: true),
                    typeES = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    MobileAlias = table.Column<string>(nullable: true),
                    IsAnonymous = table.Column<bool>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LegacyPasswordHash = table.Column<string>(nullable: true),
                    MobilePIN = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    LoweredEmail = table.Column<string>(nullable: true),
                    LoweredUserName = table.Column<string>(nullable: true),
                    PasswordQuestion = table.Column<string>(nullable: true),
                    PasswordAnswer = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsLockedOut = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLockoutDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FailedPasswordAttemptCount = table.Column<int>(nullable: false),
                    FailedPasswordAttemptWindowStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    FailedPasswordAnswerAttemptCount = table.Column<int>(nullable: false),
                    FailedPasswordAnswerAttemptWindowStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Center",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Address1 = table.Column<string>(maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    zipcode = table.Column<string>(maxLength: 10, nullable: true),
                    phone = table.Column<string>(unicode: false, maxLength: 12, nullable: true),
                    Center_contact_firstname1 = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Center_contact_lastname1 = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Center", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    key = table.Column<string>(maxLength: 50, nullable: false),
                    value = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    publicConfig = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ELMAH_Error",
                columns: table => new
                {
                    ErrorId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Application = table.Column<string>(maxLength: 60, nullable: false),
                    Host = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Source = table.Column<string>(maxLength: 60, nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: false),
                    User = table.Column<string>(maxLength: 50, nullable: false),
                    StatusCode = table.Column<int>(nullable: false),
                    TimeUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllXml = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ELMAH_Error", x => x.ErrorId);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    emailFrom = table.Column<string>(maxLength: 50, nullable: true),
                    emailTo = table.Column<string>(maxLength: 50, nullable: false),
                    subject = table.Column<string>(maxLength: 100, nullable: false),
                    body = table.Column<string>(nullable: false),
                    transmitAttempts = table.Column<int>(nullable: false),
                    statusID = table.Column<int>(nullable: false),
                    lastAttempt = table.Column<DateTime>(type: "datetime", nullable: true),
                    attachment = table.Column<string>(nullable: true),
                    attachmentContentType = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
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
                    active = table.Column<bool>(nullable: false),
                    onlineSource = table.Column<bool>(nullable: false),
                    returnCustomer = table.Column<bool>(nullable: false),
                    receiveUpdates = table.Column<bool>(nullable: false),
                    business = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    address1 = table.Column<string>(maxLength: 50, nullable: false),
                    address2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: false),
                    state = table.Column<string>(maxLength: 2, nullable: false),
                    phone = table.Column<string>(maxLength: 12, nullable: false),
                    cellphone = table.Column<string>(maxLength: 12, nullable: true),
                    zipcode = table.Column<string>(maxLength: 10, nullable: false),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    referredby = table.Column<int>(nullable: true),
                    referredbyOther = table.Column<string>(maxLength: 50, nullable: true),
                    blogparticipate = table.Column<bool>(nullable: true),
                    notes = table.Column<string>(maxLength: 4000, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    businessname = table.Column<string>(nullable: true),
                    licenseplate = table.Column<string>(maxLength: 10, nullable: true),
                    driverslicense = table.Column<string>(maxLength: 30, nullable: true),
                    onlineSigninID = table.Column<string>(maxLength: 128, nullable: true),
                    isOnlineProfileComplete = table.Column<bool>(nullable: true),
                    fax = table.Column<string>(maxLength: 12, nullable: true)
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
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(maxLength: 30, nullable: true),
                    filename = table.Column<string>(maxLength: 255, nullable: true),
                    Thumbnail = table.Column<byte[]>(nullable: true),
                    ThumbnailMimeType = table.Column<string>(maxLength: 30, nullable: true),
                    parenttable = table.Column<string>(maxLength: 30, nullable: true),
                    recordkey = table.Column<string>(maxLength: 20, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
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
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    emailTemplate = table.Column<string>(nullable: true),
                    key = table.Column<string>(maxLength: 30, nullable: true),
                    skillDescriptionEn = table.Column<string>(maxLength: 300, nullable: true),
                    skillDescriptionEs = table.Column<string>(maxLength: 300, nullable: true),
                    minimumCost = table.Column<double>(nullable: true),
                    active = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    time_stamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    host = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 50, nullable: true),
                    source = table.Column<string>(maxLength: 50, nullable: true),
                    message = table.Column<string>(nullable: false),
                    level = table.Column<string>(maxLength: 50, nullable: false),
                    logger = table.Column<string>(maxLength: 50, nullable: false),
                    stacktrace = table.Column<string>(nullable: true),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    recordID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    active = table.Column<bool>(nullable: false),
                    firstname1 = table.Column<string>(maxLength: 50, nullable: false),
                    firstname2 = table.Column<string>(maxLength: 50, nullable: true),
                    lastname1 = table.Column<string>(maxLength: 50, nullable: false),
                    lastname2 = table.Column<string>(maxLength: 50, nullable: true),
                    address1 = table.Column<string>(maxLength: 50, nullable: true),
                    address2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 25, nullable: true),
                    state = table.Column<string>(maxLength: 2, nullable: true),
                    zipcode = table.Column<string>(maxLength: 10, nullable: true),
                    phone = table.Column<string>(maxLength: 12, nullable: true),
                    gender = table.Column<int>(nullable: false),
                    genderother = table.Column<string>(maxLength: 20, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    nickname = table.Column<string>(maxLength: 50, nullable: true),
                    cellphone = table.Column<string>(maxLength: 12, nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    facebook = table.Column<string>(maxLength: 50, nullable: true),
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
                    name = table.Column<string>(nullable: true),
                    commonName = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    sqlquery = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    subcategory = table.Column<string>(nullable: true),
                    inputsJson = table.Column<string>(nullable: true),
                    columnsJson = table.Column<string>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
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
                    day = table.Column<int>(nullable: false),
                    leadHours = table.Column<int>(nullable: false),
                    minStartMin = table.Column<int>(nullable: false),
                    maxEndMin = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<string>(maxLength: 88, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime", nullable: false),
                    LockDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LockCookie = table.Column<int>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    SessionItem = table.Column<byte[]>(nullable: true),
                    Flags = table.Column<int>(nullable: false),
                    Timeout = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sessions__C9F4929003E57E5D", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "TransportProviders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    text_EN = table.Column<string>(maxLength: 50, nullable: true),
                    text_ES = table.Column<string>(maxLength: 50, nullable: true),
                    defaultAttribute = table.Column<bool>(nullable: false),
                    sortorder = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
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
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    lookupKey = table.Column<string>(maxLength: 50, nullable: true),
                    zoneLabel = table.Column<string>(maxLength: 50, nullable: true),
                    zipcodes = table.Column<string>(maxLength: 1000, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 128, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey, x.UserId });
                    table.ForeignKey(
                        name: "FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 128, nullable: false),
                    RoleId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailWorkOrders",
                columns: table => new
                {
                    Email_ID = table.Column<int>(nullable: false),
                    WorkOrder_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.EmailWorkOrders", x => new { x.Email_ID, x.WorkOrder_ID });
                    table.ForeignKey(
                        name: "FK_dbo.EmailWorkOrders_dbo.Emails_Email_ID",
                        column: x => x.Email_ID,
                        principalTable: "Emails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployerID = table.Column<int>(nullable: false),
                    onlineSource = table.Column<bool>(nullable: false),
                    paperOrderNum = table.Column<int>(nullable: true),
                    waPseudoIDCounter = table.Column<int>(nullable: false),
                    contactName = table.Column<string>(maxLength: 50, nullable: false),
                    status = table.Column<int>(nullable: false),
                    workSiteAddress1 = table.Column<string>(maxLength: 50, nullable: false),
                    workSiteAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: false),
                    state = table.Column<string>(maxLength: 2, nullable: false),
                    phone = table.Column<string>(maxLength: 12, nullable: false),
                    zipcode = table.Column<string>(maxLength: 10, nullable: false),
                    typeOfWorkID = table.Column<int>(nullable: false),
                    englishRequired = table.Column<bool>(nullable: false),
                    englishRequiredNote = table.Column<string>(maxLength: 100, nullable: true),
                    lunchSupplied = table.Column<bool>(nullable: false),
                    permanentPlacement = table.Column<bool>(nullable: false),
                    transportMethodID = table.Column<int>(nullable: false),
                    transportFee = table.Column<double>(nullable: false),
                    transportFeeExtra = table.Column<double>(nullable: false),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    dateTimeofWork = table.Column<DateTime>(type: "datetime", nullable: false),
                    timeFlexible = table.Column<bool>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    transportTransactType = table.Column<int>(nullable: true),
                    transportTransactID = table.Column<string>(maxLength: 50, nullable: true),
                    additionalNotes = table.Column<string>(maxLength: 1000, nullable: true),
                    disclosureAgreement = table.Column<bool>(nullable: true),
                    statusEN = table.Column<string>(maxLength: 50, nullable: true),
                    statusES = table.Column<string>(maxLength: 50, nullable: true),
                    transportMethodEN = table.Column<string>(nullable: true),
                    transportMethodES = table.Column<string>(nullable: true),
                    timeZoneOffset = table.Column<double>(nullable: false),
                    ppFee = table.Column<double>(nullable: true),
                    ppResponse = table.Column<string>(nullable: true),
                    ppPaymentToken = table.Column<string>(maxLength: 25, nullable: true),
                    ppPaymentID = table.Column<string>(maxLength: 50, nullable: true),
                    ppPayerID = table.Column<string>(maxLength: 25, nullable: true),
                    ppState = table.Column<string>(maxLength: 20, nullable: true),
                    transportProviderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.WorkOrders_dbo.Employers_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "Employers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySignins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    activityID = table.Column<int>(nullable: false),
                    personID = table.Column<int>(nullable: true),
                    dwccardnum = table.Column<int>(nullable: false),
                    memberStatus = table.Column<int>(nullable: true),
                    dateforsignin = table.Column<DateTime>(type: "datetime", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    timeZoneOffset = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySignins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.ActivitySignins_dbo.Activities_activityID",
                        column: x => x.activityID,
                        principalTable: "Activities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.ActivitySignins_dbo.Persons_personID",
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
                    PersonID = table.Column<int>(nullable: false),
                    eventType = table.Column<int>(nullable: false),
                    dateFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    notes = table.Column<string>(maxLength: 4000, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    eventTypeEN = table.Column<string>(maxLength: 50, nullable: true),
                    eventTypeES = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.Events_dbo.Persons_PersonID",
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
                    typeOfWorkID = table.Column<int>(nullable: false),
                    dateOfMembership = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    memberStatus = table.Column<int>(nullable: false),
                    memberReactivateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: true),
                    homeless = table.Column<bool>(nullable: true),
                    RaceID = table.Column<int>(nullable: true),
                    raceother = table.Column<string>(maxLength: 20, nullable: true),
                    height = table.Column<string>(maxLength: 50, nullable: true),
                    weight = table.Column<string>(maxLength: 10, nullable: true),
                    englishlevelID = table.Column<int>(nullable: false),
                    recentarrival = table.Column<bool>(nullable: true),
                    dateinUSA = table.Column<DateTime>(type: "datetime", nullable: true),
                    dateinseattle = table.Column<DateTime>(type: "datetime", nullable: true),
                    disabled = table.Column<bool>(nullable: true),
                    disabilitydesc = table.Column<string>(maxLength: 50, nullable: true),
                    maritalstatus = table.Column<int>(nullable: true),
                    livewithchildren = table.Column<bool>(nullable: true),
                    numofchildren = table.Column<int>(nullable: true),
                    incomeID = table.Column<int>(nullable: true),
                    livealone = table.Column<bool>(nullable: true),
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
                    memberexpirationdate = table.Column<DateTime>(type: "datetime", nullable: false),
                    driverslicense = table.Column<bool>(nullable: true),
                    licenseexpirationdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    carinsurance = table.Column<bool>(nullable: true),
                    insuranceexpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    ImageID = table.Column<int>(nullable: true),
                    skill1 = table.Column<int>(nullable: true),
                    skill2 = table.Column<int>(nullable: true),
                    skill3 = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    workerRating = table.Column<float>(nullable: true),
                    housingType = table.Column<int>(nullable: true),
                    liveWithSpouse = table.Column<bool>(nullable: true),
                    liveWithDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    americanBornChildren = table.Column<int>(nullable: true),
                    numChildrenUnder18 = table.Column<int>(nullable: true),
                    educationLevel = table.Column<int>(nullable: true),
                    farmLaborCharacteristics = table.Column<int>(nullable: true),
                    wageTheftVictim = table.Column<bool>(nullable: true),
                    wageTheftRecoveryAmount = table.Column<double>(nullable: true),
                    lastPaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastPaymentAmount = table.Column<double>(nullable: true),
                    ownTools = table.Column<bool>(nullable: true),
                    healthInsurance = table.Column<bool>(nullable: true),
                    usVeteran = table.Column<bool>(nullable: true),
                    healthInsuranceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    vehicleTypeID = table.Column<int>(nullable: true),
                    incomeSourceID = table.Column<int>(nullable: true),
                    introToCenter = table.Column<string>(maxLength: 1000, nullable: true),
                    lgbtq = table.Column<bool>(nullable: true),
                    typeOfWork = table.Column<string>(nullable: true),
                    memberStatusEN = table.Column<string>(maxLength: 50, nullable: true),
                    memberStatusES = table.Column<string>(maxLength: 50, nullable: true),
                    fullNameAndID = table.Column<string>(maxLength: 100, nullable: true),
                    skillCodes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.Workers_dbo.Persons_ID",
                        column: x => x.ID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportProviderAvailabilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    transportProviderID = table.Column<int>(nullable: false),
                    key = table.Column<string>(maxLength: 50, nullable: true),
                    lookupKey = table.Column<string>(maxLength: 50, nullable: true),
                    day = table.Column<int>(nullable: false),
                    available = table.Column<bool>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportProviderAvailabilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.TransportProviderAvailabilities_dbo.TransportProviders_transportProviderID",
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
                    transportRuleID = table.Column<int>(nullable: false),
                    minWorker = table.Column<int>(nullable: false),
                    maxWorker = table.Column<int>(nullable: false),
                    cost = table.Column<double>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCostRules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.TransportCostRules_dbo.TransportRules_transportRuleID",
                        column: x => x.transportRuleID,
                        principalTable: "TransportRules",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinEventImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventID = table.Column<int>(nullable: false),
                    ImageID = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinEventImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.JoinEventImages_dbo.Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.JoinEventImages_dbo.Images_ImageID",
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
                    WorkOrderID = table.Column<int>(nullable: false),
                    WorkerID = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.WorkerRequests_dbo.WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.WorkerRequests_dbo.Workers_WorkerID",
                        column: x => x.WorkerID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerSignins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkAssignmentID = table.Column<int>(nullable: true),
                    lottery_timestamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    lottery_sequence = table.Column<int>(nullable: true),
                    WorkerID = table.Column<int>(nullable: true),
                    dwccardnum = table.Column<int>(nullable: false),
                    memberStatus = table.Column<int>(nullable: true),
                    dateforsignin = table.Column<DateTime>(type: "datetime", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    timeZoneOffset = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerSignins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.WorkerSignins_dbo.Workers_WorkerID",
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
                    workerAssignedID = table.Column<int>(nullable: true),
                    workOrderID = table.Column<int>(nullable: false),
                    workerSigninID = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    pseudoID = table.Column<int>(nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    englishLevelID = table.Column<int>(nullable: false),
                    skillID = table.Column<int>(nullable: false),
                    surcharge = table.Column<double>(nullable: false),
                    hourlyWage = table.Column<double>(nullable: false),
                    hours = table.Column<double>(nullable: true),
                    hourRange = table.Column<int>(nullable: true),
                    days = table.Column<int>(nullable: false),
                    qualityOfWork = table.Column<int>(nullable: false),
                    followDirections = table.Column<int>(nullable: false),
                    attitude = table.Column<int>(nullable: false),
                    reliability = table.Column<int>(nullable: false),
                    transportProgram = table.Column<int>(nullable: false),
                    comments = table.Column<string>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    workerRating = table.Column<int>(nullable: true),
                    workerRatingComments = table.Column<string>(maxLength: 500, nullable: true),
                    weightLifted = table.Column<bool>(nullable: true),
                    skillEN = table.Column<string>(nullable: true),
                    skillES = table.Column<string>(nullable: true),
                    fullWAID = table.Column<string>(nullable: true),
                    minEarnings = table.Column<double>(nullable: false),
                    maxEarnings = table.Column<double>(nullable: false),
                    transportCost = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAssignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.WorkAssignments_dbo.WorkOrders_workOrderID",
                        column: x => x.workOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.WorkAssignments_dbo.Workers_workerAssignedID",
                        column: x => x.workerAssignedID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.WorkAssignments_dbo.WorkerSignins_workerSigninID",
                        column: x => x.workerSigninID,
                        principalTable: "WorkerSignins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activityID",
                table: "ActivitySignins",
                column: "activityID");

            migrationBuilder.CreateIndex(
                name: "IX_personID",
                table: "ActivitySignins",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Email_ID",
                table: "EmailWorkOrders",
                column: "Email_ID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrder_ID",
                table: "EmailWorkOrders",
                column: "WorkOrder_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonID",
                table: "Events",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_EventID",
                table: "JoinEventImages",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageID",
                table: "JoinEventImages",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Expires",
                table: "Sessions",
                column: "Expires");

            migrationBuilder.CreateIndex(
                name: "IX_transportRuleID",
                table: "TransportCostRules",
                column: "transportRuleID");

            migrationBuilder.CreateIndex(
                name: "IX_transportProviderID",
                table: "TransportProviderAvailabilities",
                column: "transportProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_workOrderID",
                table: "WorkAssignments",
                column: "workOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_workerAssignedID",
                table: "WorkAssignments",
                column: "workerAssignedID");

            migrationBuilder.CreateIndex(
                name: "IX_workerSigninID",
                table: "WorkAssignments",
                column: "workerSigninID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderID",
                table: "WorkerRequests",
                column: "WorkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerID",
                table: "WorkerRequests",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_ID",
                table: "Workers",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerID",
                table: "WorkerSignins",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "dateTimeofWork",
                table: "WorkOrders",
                column: "dateTimeofWork");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerID",
                table: "WorkOrders",
                column: "EmployerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__MigrationHistory");

            migrationBuilder.DropTable(
                name: "ActivitySignins");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "Center");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "ELMAH_Error");

            migrationBuilder.DropTable(
                name: "EmailWorkOrders");

            migrationBuilder.DropTable(
                name: "JoinEventImages");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "NLog");

            migrationBuilder.DropTable(
                name: "ReportDefinitions");

            migrationBuilder.DropTable(
                name: "ScheduleRules");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "TransportCostRules");

            migrationBuilder.DropTable(
                name: "TransportProviderAvailabilities");

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
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Images");

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
