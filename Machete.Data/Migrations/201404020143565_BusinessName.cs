namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BusinessName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employers", "businessname", c => c.String());
            Sql("CREATE TABLE [dbo].[NLog](	[Id] [int] IDENTITY(1,1) NOT NULL, [time_stamp] [datetime] NOT NULL, [host] [nvarchar](max) NULL, [type] [nvarchar](50) NULL,	[source] [nvarchar](50) NULL,	[message] [nvarchar](max) NOT NULL,	[level] [nvarchar](50) NOT NULL,	[logger] [nvarchar](50) NOT NULL,	[stacktrace] [nvarchar](max) NULL,	[username] [nvarchar](50) NULL,	[recordID] [int] NULL, CONSTRAINT [PK_NLog] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ) TEXTIMAGE_ON [PRIMARY]");
            Sql("ALTER TABLE [dbo].[NLog] ADD  CONSTRAINT [DF_NLog_time_stamp]  DEFAULT (getdate()) FOR [time_stamp]");
            Sql("CREATE TABLE [dbo].[ELMAH_Error](	[ErrorId] [uniqueidentifier] NOT NULL, [Application] [nvarchar](60) NOT NULL, [Host] [nvarchar](50) NOT NULL, [Type] [nvarchar](100) NOT NULL,	[Source] [nvarchar](60) NOT NULL,	[Message] [nvarchar](500) NOT NULL,	[User] [nvarchar](50) NOT NULL,	[StatusCode] [int] NOT NULL,	[TimeUtc] [datetime] NOT NULL,	[Sequence] [int] IDENTITY(1,1) NOT NULL,	[AllXml] [ntext] NOT NULL, CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED (	[ErrorId] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ) TEXTIMAGE_ON [PRIMARY]");
            Sql("ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId] DEFAULT (newid()) FOR [ErrorId]");
            Sql("CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml] ( @Application NVARCHAR(60), @PageIndex INT = 0, @PageSize INT = 15, @TotalCount INT OUTPUT) AS SET NOCOUNT ON DECLARE @FirstTimeUTC DATETIME    DECLARE @FirstSequence INT    DECLARE @StartRow INT    DECLARE @StartRowIndex INT    SELECT         @TotalCount = COUNT(1)     FROM         [ELMAH_Error]    WHERE         [Application] = @Application    SET @StartRowIndex = @PageIndex * @PageSize + 1    IF @StartRowIndex <= @TotalCount BEGIN SET ROWCOUNT @StartRowIndex SELECT              @FirstTimeUTC = [TimeUtc],            @FirstSequence = [Sequence]        FROM             [ELMAH_Error]        WHERE               [Application] = @Application        ORDER BY             [TimeUtc] DESC,            [Sequence] DESC    END    ELSE    BEGIN        SET @PageSize = 0    END    SET ROWCOUNT @PageSize    SELECT errorId     = [ErrorId],         application = [Application],        host        = [Host],         type        = [Type],        source      = [Source],        message     = [Message],        [user]      = [User],        statusCode  = [StatusCode],         time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'    FROM         [ELMAH_Error] error    WHERE        [Application] = @Application    AND        [TimeUtc] <= @FirstTimeUTC    AND         [Sequence] <= @FirstSequence    ORDER BY        [TimeUtc] DESC,         [Sequence] DESC    FOR        XML AUTO");
            Sql("CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml] ( @Application NVARCHAR(60), @ErrorId UNIQUEIDENTIFIER ) AS SET NOCOUNT ON SELECT [AllXml] FROM [ELMAH_Error] WHERE        [ErrorId] = @ErrorId    AND        [Application] = @Application");
            Sql("CREATE PROCEDURE [dbo].[ELMAH_LogError] ( @ErrorId UNIQUEIDENTIFIER, @Application NVARCHAR(60), @Host NVARCHAR(30), @Type NVARCHAR(100), @Source NVARCHAR(60), @Message NVARCHAR(500), @User NVARCHAR(50), @AllXml NTEXT, @StatusCode INT, @TimeUtc DATETIME ) AS SET NOCOUNT ON INSERT INTO [ELMAH_Error] ( [ErrorId], [Application], [Host], [Type], [Source], [Message], [User], [AllXml], [StatusCode], [TimeUtc] ) VALUES ( @ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @AllXml, @StatusCode, @TimeUtc)");
            // Uncomment the following lines to update a database that existed prior to the initial migration for this project
            //Sql("update dbo.Lookups set [key] = [ltrCode] where category = 'worktype'");
            //Sql("IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Organizing Meeting' AND category = 'activityType') BEGIN UPDATE dbo.Lookups SET [key] = 'Organizing Meeting' WHERE category = 'activityType' AND text_EN = 'Organizing Meeting' END IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityType' AND [key]='Organizing Meeting') BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) VALUES ('activityType', 'Organizing Meeting', 'Organizing Meeting', 'Reunión de Organizadores', 0, 0, GETDATE(), GETDATE()) END");
            //Sql("IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Organizing Meeting' AND category = 'activityName') BEGIN UPDATE dbo.Lookups SET [key] = 'Organizing Meeting' WHERE category = 'activityName' AND text_EN = 'Organizing Meeting' END IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityName' AND [key]='Organizing Meeting') BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) VALUES ('activityName', 'Organizing Meeting', 'Organizing Meeting', 'Reunión de Organizadores', 0, 0, GETDATE(), GETDATE()) END");
            //Sql("IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Assembly' AND category = 'activityType') BEGIN UPDATE dbo.Lookups SET [key] = 'Assembly' WHERE text_EN = 'Assembly' AND category = 'activityType' END IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityType' AND [key]='Assembly') BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) VALUES ('activityType', 'Assembly', 'Assembly', 'Asamblea', 0, 0, GETDATE(), GETDATE()) END");
            //Sql("IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Assembly' AND category = 'activityName') BEGIN UPDATE dbo.Lookups SET [key] = 'Assembly' WHERE category = 'activityName' AND text_EN = 'Assembly' END IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityName' AND [key]='Assembly') BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) VALUES ('activityName', 'Assembly', 'Assembly', 'Asamblea', 0, 0, GETDATE(), GETDATE()) END");
            //Sql("IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Class' AND category = 'activityType') BEGIN UPDATE dbo.Lookups SET [key] = 'Class' WHERE text_EN = 'Class' AND category = 'activityType' END IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category = 'activityType' AND [key] = 'Class') BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) VALUES ('activityType', 'Class', 'Class', 'Clase', 0, 0, GETDATE(), GETDATE()) END");
            
        }
        
        public override void Down()
        {
            // Delete the new keys, not any references that may have been created ('key' values will be reset by 'Up' method):
            //Sql("UPDATE dbo.Lookups SET [key] = null WHERE [key] = 'Assembly' OR [key] = 'Class' OR [key] = 'Organizing Meeting'");
            //Sql("update dbo.Lookups set [key] = null where category = 'worktype'");
            DropStoredProcedure("ELMAH_LogError");
            DropStoredProcedure("ELMAH_GetErrorXml");
            DropStoredProcedure("ELMAH_GetErrorsXml");
            DropTable("dbo.ELMAH_Error");   
            DropTable("dbo.NLog");
            DropColumn("dbo.Employers", "businessname");
        }
    }
}