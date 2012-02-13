/****** Script for SelectTopNRows command from SSMS  ******/

delete from [macheteStageProd].dbo.Employers
delete from [macheteStageProd].dbo.WorkOrders
delete from [macheteStageProd].dbo.WorkAssignments
delete from [macheteStageProd].dbo.WorkerRequests
delete from [macheteStageProd].dbo.workersignins
delete from [macheteStageProd].dbo.workers
delete from [macheteStageProd].dbo.Persons
delete from [macheteStageProd].dbo.images
/*delete from [macheteStageProd].dbo.Lookups*/
CREATE TABLE [macheteStageProd].[dbo].[City_zipcode_labels](
	[zipcode] [nvarchar](10) NOT NULL,
	[label] [nvarchar](30) NOT NULL,
	[seattlecitylimits] [bit] NULL
) ON [PRIMARY]
GO

insert [macheteStageProd].[dbo].[City_zipcode_labels] ([zipcode],[label],[seattlecitylimits])
                                   SELECT [zipcode],[label],[seattlecitylimits]
  FROM [macheteStageProdOld].[dbo].[City_zipcode_labels]
GO

set IDENTITY_INSERT [macheteStageProd].dbo.Employers ON
insert into [macheteStageProd].dbo.Employers ([ID],[business],[active],[name],[address1],[address2],[city],[state],[phone],[cellphone],[zipcode],[email],[referredby],[referredbyOther],[blogparticipate],[datecreated],[dateupdated],[Createdby],[Updatedby])
                              SELECT [ID],[business],[active],[name],[address1],[address2],[city],[state],[phone],[cellphone],[zipcode],[email],[referredby],[referredbyOther],[blogparticipate],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[Employers]
go
set IDENTITY_INSERT [macheteStageProd].dbo.Employers OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageProd].dbo.WorkOrders ON
insert [macheteStageProd].[dbo].[WorkOrders] ([ID],[EmployerID],[paperOrderNum],[contactName],[status],[workSiteAddress1],[workSiteAddress2],[city],[state],[phone],[zipcode],[typeOfWorkID],[englishRequired],[englishRequiredNote],[lunchSupplied],[permanentPlacement],[transportMethodID],[transportFee],[transportFeeExtra],[description],[dateTimeofWork],[timeFlexible],[datecreated],[dateupdated],[Createdby],[Updatedby],[waPseudoIDCounter])
							           SELECT [ID],[EmployerID],[paperOrderNum],[contactName],[status],[workSiteAddress1],[workSiteAddress2],[city],[state],[phone],[zipcode],[typeOfWorkID],[englishRequired],[englishRequiredNote],[lunchSupplied],[permanentPlacement],[transportMethodID],[transportFee],[transportFeeExtra],[description],[dateTimeofWork],[timeFlexible],[datecreated],[dateupdated],[Createdby],[Updatedby],[waPseudoIDCounter]
  FROM [macheteStageProdOld].[dbo].[WorkOrders]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.WorkOrders OFF
GO
/**************************************************/
set IDENTITY_INSERT [macheteStageProd].dbo.Persons ON
insert [macheteStageProd].dbo.persons ([ID],[active],[firstname1],[firstname2],[lastname1],[lastname2],[address1],[address2],[city],[state],[zipcode],[phone],[gender],[genderother],[datecreated],[dateupdated],[Createdby],[Updatedby])
                       SELECT [ID],[active],[firstname1],[firstname2],[lastname1],[lastname2],[address1],[address2],[city],[state],[zipcode],[phone],[gender],[genderother],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[Persons]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.Persons OFF
go
/*************************************************
TEMP: mapping worker.active to worker.memberstatus
*/
insert [macheteStageProd].dbo.workers ([ID],[memberstatus],                                  [typeOfWorkID],[dateOfMembership],[dateOfBirth],  [active],[RaceID],[raceother],[height],[weight],[englishlevelID],[recentarrival],[dateinUSA],[dateinseattle],[disabled],[disabilitydesc],[maritalstatus],[livewithchildren],[numofchildren],[incomeID],[livealone],[emcontUSAname],[emcontUSArelation],[emcontUSAphone],[dwccardnum],[neighborhoodID],[immigrantrefugee],[countryoforiginID],[emcontoriginname],[emcontoriginrelation],[emcontoriginphone],[memberexpirationdate],[driverslicense],[licenseexpirationdate],[carinsurance],[insuranceexpiration],[ImageID],[skill1],[skill2],[skill3],[datecreated],[dateupdated],[Createdby],[Updatedby])
                     SELECT w.[ID],case w.active when 1 then 93 when 0 then 94 end, [typeOfWorkID],[dateOfMembership],[dateOfBirth],w.[active],[RaceID],[raceother],[height],[weight],[englishlevelID],[recentarrival],[dateinUSA],[dateinseattle],[disabled],[disabilitydesc],[maritalstatus],[livewithchildren],[numofchildren],[incomeID],[livealone],[emcontUSAname],[emcontUSArelation],[emcontUSAphone],[dwccardnum],[neighborhoodID],[immigrantrefugee],[countryoforiginID],[emcontoriginname],[emcontoriginrelation],[emcontoriginphone],[memberexpirationdate],[driverslicense],[licenseexpirationdate],[carinsurance],[insuranceexpiration],[ImageID],[skill1],[skill2],[skill3],w.[datecreated],w.[dateupdated],w.[Createdby],w.[Updatedby]
  FROM [macheteStageProdOld].[dbo].[Workers] w join machetestageprod.dbo.persons p on p.ID = w.ID
GO
/**************************************************/
set IDENTITY_INSERT [macheteStageProd].dbo.WorkerSignins ON
insert [macheteStageProd].dbo.workersignins ([ID],[dwccardnum],[WorkerID],[WorkAssignmentID],[dateforsignin],[lottery_timestamp],[datecreated],[dateupdated],[Createdby],[Updatedby])
                             SELECT [ID],[dwccardnum],[WorkerID],[WorkAssignmentID],[dateforsignin],[lottery_timestamp],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[WorkerSignins]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.WorkerSignins OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageProd].dbo.Images ON
insert [macheteStageProd].dbo.images ([ID],[ImageData],[ImageMimeType],[filename],[Thumbnail],[ThumbnailMimeType],[parenttable],[recordkey],[datecreated],[dateupdated],[Createdby],[Updatedby])
                      SELECT [ID],[ImageData],[ImageMimeType],[filename],[Thumbnail],[ThumbnailMimeType],[parenttable],[recordkey],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[Images]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.Images OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageProd].dbo.workerrequests ON
insert [macheteStageProd].dbo.workerrequests ([ID],[WorkOrderID],[WorkerID],[datecreated],[dateupdated],[Createdby],[Updatedby])
                              SELECT [ID],[WorkOrderID],[WorkerID],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[WorkerRequests]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.workerrequests OFF
go
/*************************************************
TEMP: hourRange
*/
set IDENTITY_INSERT [macheteStageProd].dbo.WorkAssignments ON
insert [macheteStageProd].[dbo].[WorkAssignments] ([ID],[workerAssignedID],[workOrderID],[workerSigninID],[active],[pseudoID],[description],[englishLevelID],[skillID],[hourlyWage],[hours],[hourRange],[days],[qualityOfWork],[followDirections],[attitude],[reliability],[transportProgram],[comments],[datecreated],[dateupdated],[Createdby],[Updatedby])
                                   SELECT [ID],[workerAssignedID],[workOrderID],[workerSigninID],[active],[pseudoID],[description],[englishLevelID],[skillID],[hourlyWage],[hours],          0,[days],[qualityOfWork],[followDirections],[attitude],[reliability],[transportProgram],[comments],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProdOld].[dbo].[WorkAssignments]
GO
set IDENTITY_INSERT [macheteStageProd].dbo.WorkAssignments OFF
go

/**************************************************/
/*
set IDENTITY_INSERT [macheteStageProd].dbo.lookups ON
insert [macheteStageProd].dbo.Lookups ([ID],[category],[text_EN],[text_ES],[selected],[subcategory],[level],[wage],[minHour],[fixedJob],[sortorder],[typeOfWorkID],[speciality],[ltrCode])
                       SELECT [ID],[category],[text_EN],[text_ES],[selected],[subcategory],[level],[wage],[minHour],[fixedJob],[sortorder],[typeOfWorkID],[speciality],[ltrCode]
  FROM [macheteStageProdOld].[dbo].[Lookups]
go
set IDENTITY_INSERT [macheteStageProd].dbo.lookups Off
go
*/