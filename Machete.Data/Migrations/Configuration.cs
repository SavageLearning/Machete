namespace Machete.Data.Migrations
{
    using Machete.Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Machete.Data.MacheteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        bool AddUserAndRole(MacheteContext context)
        {
            IdentityResult ir;
            //uncomment the following lines to create new role types for users
            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(context));
            //ir = rm.Create(new IdentityRole("Administrator"));
            //ir = rm.Create(new IdentityRole("Manager"));
            //ir = rm.Create(new IdentityRole("Check-in"));
            //ir = rm.Create(new IdentityRole("PhoneDesk"));
            //ir = rm.Create(new IdentityRole("Teacher"));
            //ir = rm.Create(new IdentityRole("User"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "",
                IsApproved = true,
                Email = ""
            };
            ir = um.Create(user, "");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Administrator"); //Default Administrator, edit to change
            return ir.Succeeded;
        }

        protected override void Seed(Machete.Data.MacheteContext context)
        {
            // Add a user:
            //var result = AddUserAndRole(context);
            
            // Create the Lookups table: (ONLY DO THIS ONCE)
            //MacheteLookup.Initialize(context);
            //context.SaveChanges();
            
            // Why this, I don't know. It was in the MacheteInitializer.cs file. Run it once.
            //context.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX [dateTimeofWork] ON [dbo].[WorkOrders] ([dateTimeofWork] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

            // Create NLog table:
            //context.Database.ExecuteSqlCommand("CREATE TABLE [dbo].[NLog]([Id] [int] IDENTITY(1,1) NOT NULL, [time_stamp] [datetime] NOT NULL, [host] [nvarchar](max) NULL, [type] [nvarchar](50) NULL, [source] [nvarchar](50) NULL, [message] [nvarchar](max) NOT NULL, [level] [nvarchar](50) NOT NULL, [logger] [nvarchar](50) NOT NULL, [stacktrace] [nvarchar](max) NULL, [username] [nvarchar](50) NULL, [recordID] [int] NULL, CONSTRAINT [PK_NLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            // GO
            //context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[NLog] ADD  CONSTRAINT [DF_NLog_time_stamp]  DEFAULT (getdate()) FOR [time_stamp]");
            //
            //
            // Create Elmah table:
            //context.Database.ExecuteSqlCommand("CREATE TABLE [dbo].[ELMAH_Error] (	[ErrorId] [uniqueidentifier] NOT NULL,	[Application] [nvarchar](60) NOT NULL,	[Host] [nvarchar](50) NOT NULL,	[Type] [nvarchar](100) NOT NULL,	[Source] [nvarchar](60) NOT NULL,	[Message] [nvarchar](500) NOT NULL,	[User] [nvarchar](50) NOT NULL,	[StatusCode] [int] NOT NULL,	[TimeUtc] [datetime] NOT NULL,	[Sequence] [int] IDENTITY(1,1) NOT NULL,	[AllXml] [ntext] NOT NULL, CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED (	[ErrorId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            // GO
            //context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]");
            // GO
            //
            //
            // Elmah Stored Procedure 1
            //context.Database.ExecuteSqlCommand("CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml] (    @Application NVARCHAR(60),    @PageIndex INT = 0,    @PageSize INT = 15,    @TotalCount INT OUTPUT ) AS     SET NOCOUNT ON    DECLARE @FirstTimeUTC DATETIME    DECLARE @FirstSequence INT    DECLARE @StartRow INT   DECLARE @StartRowIndex INT    SELECT         @TotalCount = COUNT(1)     FROM         [ELMAH_Error]    WHERE         [Application] = @Application        SET @StartRowIndex = @PageIndex * @PageSize + 1    IF @StartRowIndex <= @TotalCount    BEGIN        SET ROWCOUNT @StartRowIndex        SELECT              @FirstTimeUTC = [TimeUtc],            @FirstSequence = [Sequence]        FROM             [ELMAH_Error]        WHERE               [Application] = @Application        ORDER BY             [TimeUtc] DESC,             [Sequence] DESC    END    ELSE    BEGIN        SET @PageSize = 0    END       SET ROWCOUNT @PageSize   SELECT         errorId     = [ErrorId],         application = [Application],        host        = [Host],         type        = [Type],        source      = [Source],        message     = [Message],        [user]      = [User],        statusCode  = [StatusCode],         time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'    FROM         [ELMAH_Error] error    WHERE        [Application] = @Application    AND        [TimeUtc] <= @FirstTimeUTC    AND         [Sequence] <= @FirstSequence    ORDER BY        [TimeUtc] DESC,         [Sequence] DESC    FOR        XML AUTO");
            //GO
            //
            // Elmah Stored Procedure 2
            //context.Database.ExecuteSqlCommand("CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml] (    @Application NVARCHAR(60),    @ErrorId UNIQUEIDENTIFIER) AS     SET NOCOUNT ON    SELECT         [AllXml]    FROM         [ELMAH_Error]    WHERE        [ErrorId] = @ErrorId    AND        [Application] = @Application");
            //GO
            //
            // Elmah Stored Procedure 3
            //context.Database.ExecuteSqlCommand("CREATE PROCEDURE [dbo].[ELMAH_LogError] (    @ErrorId UNIQUEIDENTIFIER,    @Application NVARCHAR(60),    @Host NVARCHAR(30),    @Type NVARCHAR(100),    @Source NVARCHAR(60),    @Message NVARCHAR(500),    @User NVARCHAR(50),    @AllXml NTEXT,    @StatusCode INT,    @TimeUtc DATETIME) AS    SET NOCOUNT ON    INSERT    INTO        [ELMAH_Error]        (            [ErrorId],            [Application],            [Host],            [Type],            [Source],            [Message],            [User],            [AllXml],            [StatusCode],            [TimeUtc]        )    VALUES        (            @ErrorId,            @Application,            @Host,            @Type,            @Source,            @Message,            @User,            @AllXml,            @StatusCode,            @TimeUtc        )");
            //GO
        }
    }
}
