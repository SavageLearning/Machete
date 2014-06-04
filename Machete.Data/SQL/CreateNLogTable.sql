CREATE TABLE [dbo].[NLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[time_stamp] [datetime] NOT NULL,
	[host] [nvarchar](max) NULL,
	[type] [nvarchar](50) NULL,
	[source] [nvarchar](50) NULL,
	[message] [nvarchar](max) NOT NULL,
	[level] [nvarchar](50) NOT NULL,
	[logger] [nvarchar](50) NOT NULL,
	[stacktrace] [nvarchar](max) NULL,
	[username] [nvarchar](50) NULL,
	[recordID] [int] NULL,
 CONSTRAINT [PK_NLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
-- Does not work in SQL Azure:
--ON [PRIMARY]
)
--ON [PRIMARY] 
TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[NLog] ADD  CONSTRAINT [DF_NLog_time_stamp]  DEFAULT (getdate()) FOR [time_stamp]
GO


