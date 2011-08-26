/****** Script for SelectTopNRows command from SSMS  ******/

delete from [macheteStageTest].dbo.Employers
delete from [macheteStageTest].dbo.WorkOrders
delete from [macheteStageTest].dbo.WorkAssignments
delete from [macheteStageTest].dbo.WorkerRequests
delete from [macheteStageTest].dbo.workersignins
delete from [macheteStageTest].dbo.workers
delete from [macheteStageTest].dbo.Persons
delete from [macheteStageTest].dbo.images
delete from [macheteStageTest].dbo.Lookups

go
set IDENTITY_INSERT [macheteStageTest].dbo.Employers ON
insert into [macheteStageTest].dbo.Employers 
      ([ID],[business],[active],[name],[address1],[address2],[city],[state],[phone],[cellphone],[zipcode],[email],[referredby],[referredbyOther],[blogparticipate],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],0,         [active],[name],[address1],[address2],[city],[state],[phone],[cellphone],[zipcode],[email],[referredby],[referredbyOther],'0',[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProd].[dbo].[Employers]
go
set IDENTITY_INSERT [macheteStageTest].dbo.Employers OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.WorkOrders ON
insert [macheteStageTest].[dbo].[WorkOrders] 
      ([ID],[EmployerID],[paperOrderNum],[contactName],[status],[workSiteAddress1],[workSiteAddress2],[city],[state],[phone],[zipcode],[typeOfWorkID],[englishRequired],[englishRequiredNote],[lunchSupplied],[permanentPlacement],[transportMethodID],[transportFee],[transportFeeExtra],[description],[dateTimeofWork],[timeFlexible],[datecreated],[dateupdated],[Createdby],[Updatedby], [waPseudoIDCounter])
SELECT [ID],[EmployerID],[paperOrderNum],[contactName],[status],[workSiteAddress1],[workSiteAddress2],[city],[state],[phone],[zipcode],[typeOfWorkID],[englishRequired],[englishRequiredNote],[lunchSupplied],[permanentPlacement],[transportMethodID],[transportFee],[transportFeeExtra],[description],[dateTimeofWork],[timeFlexible],[datecreated],[dateupdated],[Createdby],[Updatedby],[waPseudoIDCounter]
  FROM [macheteStageProd].[dbo].[WorkOrders]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.WorkOrders OFF
GO

/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.Persons ON
insert [macheteStageTest].dbo.persons 
      ([ID],[active],[firstname1],[firstname2],[lastname1],[lastname2],[address1],[address2],[city],[state],[zipcode],[phone],[gender],[genderother],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],[active],[firstname1],[firstname2],[lastname1],[lastname2],[address1],[address2],[city],[state],[zipcode],[phone],[gender],[genderother],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProd].[dbo].[Persons]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.Persons OFF
go
/**************************************************/
insert [macheteStageTest].dbo.workers 
        ([ID],[typeOfWorkID],[dateOfMembership],[dateOfBirth],  [active],[RaceID],[raceother],[height],[weight],[englishlevelID],[recentarrival],[dateinUSA],[dateinseattle],[disabled],[disabilitydesc],[maritalstatus],[livewithchildren],[numofchildren],[incomeID],[livealone],[emcontUSAname],[emcontUSArelation],[emcontUSAphone],[dwccardnum],[neighborhoodID],[immigrantrefugee],[countryoforiginID],[emcontoriginname],[emcontoriginrelation],[emcontoriginphone],[memberexpirationdate],[driverslicense],[licenseexpirationdate],[carinsurance],[insuranceexpiration],[ImageID],[skill1],[skill2],[skill3],  [datecreated],  [dateupdated],  [Createdby],  [Updatedby])
SELECT w.[ID],[typeOfWorkID],[dateOfMembership],[dateOfBirth],w.[active],[RaceID],[raceother],[height],[weight],[englishlevelID],[recentarrival],[dateinUSA],[dateinseattle],[disabled],[disabilitydesc],[maritalstatus],[livewithchildren],[numofchildren],[incomeID],[livealone],[emcontUSAname],[emcontUSArelation],[emcontUSAphone],[dwccardnum],[neighborhoodID],[immigrantrefugee],[countryoforiginID],[emcontoriginname],[emcontoriginrelation],[emcontoriginphone],[memberexpirationdate],[driverslicense],[licenseexpirationdate],[carinsurance],[insuranceexpiration],[ImageID],[skill1],[skill2],[skill3],w.[datecreated],w.[dateupdated],w.[Createdby],w.[Updatedby]
  FROM [macheteStageProd].[dbo].[Workers] w join machetestageprod.dbo.persons p on p.ID = w.ID
GO
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.WorkerSignins ON
insert [macheteStageTest].dbo.workersignins 
      ([ID],[dwccardnum],[WorkerID],[WorkAssignmentID],[dateforsignin],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],[dwccardnum],[WorkerID],[WorkAssignmentID],[dateforsignin],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProd].[dbo].[WorkerSignins]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.WorkerSignins OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.Images ON
insert [macheteStageTest].dbo.images 
      ([ID],[ImageData],[ImageMimeType],[filename],[Thumbnail],[ThumbnailMimeType],[parenttable],[recordkey],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],[ImageData],[ImageMimeType],[filename],[Thumbnail],[ThumbnailMimeType],[parenttable],[recordkey],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [machetestageprod].[dbo].[Images]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.Images OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.workerrequests ON
insert [macheteStageTest].dbo.workerrequests 
      ([ID],[WorkOrderID],[WorkerID],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],[WorkOrderID],[WorkerID],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProd].[dbo].[WorkerRequests]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.workerrequests OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.WorkAssignments ON
insert [macheteStageTest].[dbo].[WorkAssignments] 
      ([ID],[workerAssignedID],[workOrderID],[workerSigninID],[active],[pseudoID],[description],[englishLevelID],[skillID],[hourlyWage],[hours],[days],[qualityOfWork],[followDirections],[attitude],[reliability],[transportProgram],[comments],[datecreated],[dateupdated],[Createdby],[Updatedby])
SELECT [ID],[workerAssignedID],[workOrderID],[workerSigninID],[active],[pseudoID],[description],[englishLevelID],[skillID],[hourlyWage],[hours],[days],[qualityOfWork],[followDirections],[attitude],[reliability],[transportProgram],[comments],[datecreated],[dateupdated],[Createdby],[Updatedby]
  FROM [macheteStageProd].[dbo].[WorkAssignments]
GO
set IDENTITY_INSERT [macheteStageTest].dbo.WorkAssignments OFF
go
/**************************************************/
set IDENTITY_INSERT [macheteStageTest].dbo.lookups ON
insert [macheteStageTest].[dbo].lookups 
      ([ID],[category],[text_EN],[text_ES],[selected],[subcategory],[level],[wage],[minHour],[fixedJob],[sortorder],[typeOfWorkID],[speciality])
SELECT [ID],[category],[text_EN],[text_ES],[selected],[subcategory],[level],[wage],[minHour],[fixedJob],[sortorder],[typeOfWorkID],[speciality]
  FROM [macheteStageProd].[dbo].lookups
GO
set IDENTITY_INSERT [macheteStageTest].dbo.lookups OFF
go