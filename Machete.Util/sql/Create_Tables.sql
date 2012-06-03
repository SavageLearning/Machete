USE [machete]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [int] NOT NULL,
	[type] [int] NOT NULL,
	[dateStart] [datetime] NOT NULL,
	[dateEnd] [datetime] NOT NULL,
	[teacher] [nvarchar](max) NOT NULL,
	[notes] [nvarchar](4000) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employers]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[active] [bit] NOT NULL,
	[business] [bit] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[address1] [nvarchar](50) NOT NULL,
	[address2] [nvarchar](50) NULL,
	[city] [nvarchar](50) NOT NULL,
	[state] [nvarchar](2) NOT NULL,
	[phone] [nvarchar](12) NOT NULL,
	[cellphone] [nvarchar](12) NULL,
	[zipcode] [nvarchar](10) NOT NULL,
	[email] [nvarchar](50) NULL,
	[referredby] [int] NULL,
	[referredbyOther] [nvarchar](50) NULL,
	[blogparticipate] [bit] NOT NULL,
	[notes] [nvarchar](4000) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EdmMetadata]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EdmMetadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModelHash] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Images]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Images](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ImageData] [varbinary](max) NULL,
	[ImageMimeType] [nvarchar](30) NULL,
	[filename] [nvarchar](255) NULL,
	[Thumbnail] [varbinary](max) NULL,
	[ThumbnailMimeType] [nvarchar](30) NULL,
	[parenttable] [nvarchar](30) NULL,
	[recordkey] [nvarchar](20) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Persons]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[active] [bit] NOT NULL,
	[firstname1] [nvarchar](50) NOT NULL,
	[firstname2] [nvarchar](50) NULL,
	[lastname1] [nvarchar](50) NOT NULL,
	[lastname2] [nvarchar](50) NULL,
	[address1] [nvarchar](50) NULL,
	[address2] [nvarchar](50) NULL,
	[city] [nvarchar](25) NULL,
	[state] [nvarchar](2) NULL,
	[zipcode] [nvarchar](10) NULL,
	[phone] [nvarchar](12) NULL,
	[gender] [int] NOT NULL,
	[genderother] [nvarchar](20) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lookups]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[category] [nvarchar](50) NULL,
	[text_EN] [nvarchar](50) NULL,
	[text_ES] [nvarchar](50) NULL,
	[selected] [bit] NOT NULL,
	[subcategory] [nvarchar](50) NULL,
	[level] [int] NULL,
	[wage] [float] NULL,
	[minHour] [int] NULL,
	[fixedJob] [bit] NULL,
	[sortorder] [int] NOT NULL,
	[typeOfWorkID] [int] NOT NULL,
	[speciality] [bit] NOT NULL,
	[ltrCode] [nvarchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Workers]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workers](
	[ID] [int] NOT NULL,
	[typeOfWorkID] [int] NOT NULL,
	[dateOfMembership] [datetime] NOT NULL,
	[dateOfBirth] [datetime] NOT NULL,
	[memberStatus] [int] NOT NULL,
	[memberReactivateDate] [datetime] NULL,
	[active] [bit] NOT NULL,
	[homeless] [bit] NULL,
	[RaceID] [int] NOT NULL,
	[raceother] [nvarchar](20) NULL,
	[height] [nvarchar](50) NOT NULL,
	[weight] [nvarchar](10) NOT NULL,
	[englishlevelID] [int] NOT NULL,
	[recentarrival] [bit] NOT NULL,
	[dateinUSA] [datetime] NOT NULL,
	[dateinseattle] [datetime] NOT NULL,
	[disabled] [bit] NOT NULL,
	[disabilitydesc] [nvarchar](50) NULL,
	[maritalstatus] [int] NOT NULL,
	[livewithchildren] [bit] NOT NULL,
	[numofchildren] [int] NOT NULL,
	[incomeID] [int] NOT NULL,
	[livealone] [bit] NOT NULL,
	[emcontUSAname] [nvarchar](50) NULL,
	[emcontUSArelation] [nvarchar](30) NULL,
	[emcontUSAphone] [nvarchar](14) NULL,
	[dwccardnum] [int] NOT NULL,
	[neighborhoodID] [int] NOT NULL,
	[immigrantrefugee] [bit] NOT NULL,
	[countryoforiginID] [int] NOT NULL,
	[emcontoriginname] [nvarchar](50) NULL,
	[emcontoriginrelation] [nvarchar](30) NULL,
	[emcontoriginphone] [nvarchar](14) NULL,
	[memberexpirationdate] [datetime] NOT NULL,
	[driverslicense] [bit] NOT NULL,
	[licenseexpirationdate] [datetime] NULL,
	[carinsurance] [bit] NULL,
	[insuranceexpiration] [datetime] NULL,
	[ImageID] [int] NULL,
	[skill1] [int] NULL,
	[skill2] [int] NULL,
	[skill3] [int] NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[eventType] [int] NOT NULL,
	[dateFrom] [datetime] NOT NULL,
	[dateTo] [datetime] NULL,
	[notes] [nvarchar](4000) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrders]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployerID] [int] NOT NULL,
	[paperOrderNum] [int] NULL,
	[waPseudoIDCounter] [int] NOT NULL,
	[contactName] [nvarchar](50) NOT NULL,
	[status] [int] NOT NULL,
	[workSiteAddress1] [nvarchar](50) NOT NULL,
	[workSiteAddress2] [nvarchar](50) NULL,
	[city] [nvarchar](50) NOT NULL,
	[state] [nvarchar](2) NOT NULL,
	[phone] [nvarchar](12) NOT NULL,
	[zipcode] [nvarchar](10) NOT NULL,
	[typeOfWorkID] [int] NOT NULL,
	[englishRequired] [bit] NOT NULL,
	[englishRequiredNote] [nvarchar](100) NULL,
	[lunchSupplied] [bit] NOT NULL,
	[permanentPlacement] [bit] NOT NULL,
	[transportMethodID] [int] NOT NULL,
	[transportFee] [float] NOT NULL,
	[transportFeeExtra] [float] NOT NULL,
	[description] [nvarchar](4000) NULL,
	[dateTimeofWork] [datetime] NOT NULL,
	[timeFlexible] [bit] NOT NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkerSignins]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkerSignins](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[dwccardnum] [int] NOT NULL,
	[WorkerID] [int] NULL,
	[memberStatus] [int] NULL,
	[WorkAssignmentID] [int] NULL,
	[dateforsignin] [datetime] NOT NULL,
	[lottery_timestamp] [datetime] NULL,
	[lottery_sequence] [int] NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivitySignins]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivitySignins](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[activityID] [int] NOT NULL,
	[dwccardnum] [int] NOT NULL,
	[WorkerID] [int] NULL,
	[memberStatus] [int] NULL,
	[WorkAssignmentID] [int] NULL,
	[dateforsignin] [datetime] NOT NULL,
	[lottery_timestamp] [datetime] NULL,
	[lottery_sequence] [int] NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JoinEventImages]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JoinEventImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[ImageID] [int] NOT NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkerRequests]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkerRequests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderID] [int] NOT NULL,
	[WorkerID] [int] NOT NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkAssignments]    Script Date: 03/19/2012 20:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkAssignments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[workerAssignedID] [int] NULL,
	[workOrderID] [int] NOT NULL,
	[workerSigninID] [int] NULL,
	[active] [bit] NOT NULL,
	[pseudoID] [int] NULL,
	[description] [nvarchar](1000) NULL,
	[englishLevelID] [int] NOT NULL,
	[skillID] [int] NOT NULL,
	[hourlyWage] [float] NOT NULL,
	[hours] [int] NOT NULL,
	[hourRange] [int] NULL,
	[days] [int] NOT NULL,
	[qualityOfWork] [int] NOT NULL,
	[followDirections] [int] NOT NULL,
	[attitude] [int] NOT NULL,
	[reliability] [int] NOT NULL,
	[transportProgram] [int] NOT NULL,
	[comments] [nvarchar](max) NULL,
	[datecreated] [datetime] NOT NULL,
	[dateupdated] [datetime] NOT NULL,
	[Createdby] [nvarchar](30) NULL,
	[Updatedby] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [Activity_Signins]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[ActivitySignins]  WITH CHECK ADD  CONSTRAINT [Activity_Signins] FOREIGN KEY([activityID])
REFERENCES [dbo].[Activities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivitySignins] CHECK CONSTRAINT [Activity_Signins]
GO
/****** Object:  ForeignKey [ActivitySignin_worker]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[ActivitySignins]  WITH CHECK ADD  CONSTRAINT [ActivitySignin_worker] FOREIGN KEY([WorkerID])
REFERENCES [dbo].[Workers] ([ID])
GO
ALTER TABLE [dbo].[ActivitySignins] CHECK CONSTRAINT [ActivitySignin_worker]
GO
/****** Object:  ForeignKey [Event_Person]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [Event_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Persons] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [Event_Person]
GO
/****** Object:  ForeignKey [JoinEventImage_Event]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[JoinEventImages]  WITH CHECK ADD  CONSTRAINT [JoinEventImage_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JoinEventImages] CHECK CONSTRAINT [JoinEventImage_Event]
GO
/****** Object:  ForeignKey [JoinEventImage_Image]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[JoinEventImages]  WITH CHECK ADD  CONSTRAINT [JoinEventImage_Image] FOREIGN KEY([ImageID])
REFERENCES [dbo].[Images] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JoinEventImages] CHECK CONSTRAINT [JoinEventImage_Image]
GO
/****** Object:  ForeignKey [WorkAssignment_workerSiginin]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkAssignments]  WITH CHECK ADD  CONSTRAINT [WorkAssignment_workerSiginin] FOREIGN KEY([workerSigninID])
REFERENCES [dbo].[WorkerSignins] ([ID])
GO
ALTER TABLE [dbo].[WorkAssignments] CHECK CONSTRAINT [WorkAssignment_workerSiginin]
GO
/****** Object:  ForeignKey [Worker_workAssignments]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkAssignments]  WITH CHECK ADD  CONSTRAINT [Worker_workAssignments] FOREIGN KEY([workerAssignedID])
REFERENCES [dbo].[Workers] ([ID])
GO
ALTER TABLE [dbo].[WorkAssignments] CHECK CONSTRAINT [Worker_workAssignments]
GO
/****** Object:  ForeignKey [WorkOrder_workAssignments]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkAssignments]  WITH CHECK ADD  CONSTRAINT [WorkOrder_workAssignments] FOREIGN KEY([workOrderID])
REFERENCES [dbo].[WorkOrders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkAssignments] CHECK CONSTRAINT [WorkOrder_workAssignments]
GO
/****** Object:  ForeignKey [WorkerRequest_workerRequested]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkerRequests]  WITH CHECK ADD  CONSTRAINT [WorkerRequest_workerRequested] FOREIGN KEY([WorkerID])
REFERENCES [dbo].[Workers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkerRequests] CHECK CONSTRAINT [WorkerRequest_workerRequested]
GO
/****** Object:  ForeignKey [WorkerRequest_workOrder]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkerRequests]  WITH CHECK ADD  CONSTRAINT [WorkerRequest_workOrder] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkerRequests] CHECK CONSTRAINT [WorkerRequest_workOrder]
GO
/****** Object:  ForeignKey [Person_Worker]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[Workers]  WITH CHECK ADD  CONSTRAINT [Person_Worker] FOREIGN KEY([ID])
REFERENCES [dbo].[Persons] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Workers] CHECK CONSTRAINT [Person_Worker]
GO
/****** Object:  ForeignKey [Worker_workersignins]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkerSignins]  WITH CHECK ADD  CONSTRAINT [Worker_workersignins] FOREIGN KEY([WorkerID])
REFERENCES [dbo].[Workers] ([ID])
GO
ALTER TABLE [dbo].[WorkerSignins] CHECK CONSTRAINT [Worker_workersignins]
GO
/****** Object:  ForeignKey [Employer_WorkOrders]    Script Date: 03/19/2012 20:07:28 ******/
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [Employer_WorkOrders] FOREIGN KEY([EmployerID])
REFERENCES [dbo].[Employers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [Employer_WorkOrders]
GO
