CREATE MASTER KEY ENCRYPTION BY PASSWORD = ''
GO
-- DROP MASTER KEY

CREATE DATABASE SCOPED CREDENTIAL remoteCredential WITH IDENTITY = '', SECRET = ''
GO
-- DROP DATABASE SCOPED CREDENTIAL remoteCredential

CREATE EXTERNAL DATA SOURCE pomonaBackup WITH (
  TYPE = RDBMS ,
  LOCATION = 'server.database.windows.net',
  DATABASE_NAME = 'database_name',
  CREDENTIAL = remoteCredential
)
GO
-- DROP EXTERNAL DATA SOURCE pomonaBackup

CREATE EXTERNAL TABLE [Employers2018] (
  ID                      int          not null,
  active                  bit          not null,
  onlineSource            bit          not null,
  returnCustomer          bit          not null,
  receiveUpdates          bit          not null,
  business                bit          not null,
  name                    nvarchar(50) not null,
  address1                nvarchar(50) not null,
  address2                nvarchar(50),
  city                    nvarchar(50) not null,
  state                   nvarchar(2)  not null,
  phone                   nvarchar(12) not null,
  cellphone               nvarchar(12),
  zipcode                 nvarchar(10) not null,
  email                   nvarchar(50),
  referredby              int,
  referredbyOther         nvarchar(50),
  blogparticipate         bit,
  notes                   nvarchar(4000),
  datecreated             datetime     not null,
  dateupdated             datetime     not null,
  Createdby               nvarchar(30),
  Updatedby               nvarchar(30),
  businessname            nvarchar(max),
  licenseplate            nvarchar(10),
  driverslicense          nvarchar(30),
  onlineSigninID          nvarchar(128),
  isOnlineProfileComplete bit,
  fax                     nvarchar(12)
) WITH (
  DATA_SOURCE = pomonaBackup
)
-- DROP EXTERNAL TABLE Employers2018

CREATE EXTERNAL TABLE [WorkOrders2018] (
  ID                    int             not null,
  EmployerID            int             not null,
--    constraint [FK_dbo.WorkOrders_dbo.Employers_EmployerID]
--    references Employers2018
--      on delete cascade,
  onlineSource          bit             not null,
  paperOrderNum         int,
  waPseudoIDCounter     int             not null,
  contactName           nvarchar(50)    not null,
  status                int             not null,
  workSiteAddress1      nvarchar(50)    not null,
  workSiteAddress2      nvarchar(50),
  city                  nvarchar(50)    not null,
  state                 nvarchar(2)     not null,
  phone                 nvarchar(12)    not null,
  zipcode               nvarchar(10)    not null,
  typeOfWorkID          int             not null,
  englishRequired       bit             not null,
  englishRequiredNote   nvarchar(100),
  lunchSupplied         bit             not null,
  permanentPlacement    bit             not null,
  transportMethodID     int             not null,
  transportFee          float           not null,
  transportFeeExtra     float           not null,
  description           nvarchar(4000),
  dateTimeofWork        datetime        not null,
  timeFlexible          bit             not null,
  datecreated           datetime        not null,
  dateupdated           datetime        not null,
  Createdby             nvarchar(30),
  Updatedby             nvarchar(30),
  transportTransactType int,
  transportTransactID   nvarchar(50),
  additionalNotes       nvarchar(1000),
  disclosureAgreement   bit,
  paypalFee             float,
  paypalToken           nvarchar(20),
  paypalPayerId         nvarchar(15),
  paypalErrors          nvarchar(1000),
  paypalTransactID      nvarchar(50),
  statusEN              nvarchar(50),
  statusES              nvarchar(50),
  transportMethodEN     nvarchar(max),
  transportMethodES     nvarchar(max),
  timeZoneOffset        float not null --default 0 not null
) WITH (
  DATA_SOURCE = pomonaBackup
)
-- DROP EXTERNAL TABLE WorkOrders2018

CREATE EXTERNAL TABLE [WorkAssignments2018] (
  ID                   int             not null,
  workerAssignedID     int,
--    constraint [FK_dbo.WorkAssignments_dbo.Workers_workerAssignedID]
--    references Workers,
  workOrderID          int             not null,
--    constraint [FK_dbo.WorkAssignments_dbo.WorkOrders_workOrderID]
--    references WorkOrders2018
--      on delete cascade,
  workerSigninID       int,
--    constraint [FK_dbo.WorkAssignments_dbo.WorkerSignins_workerSigninID]
--    references WorkerSignins,
  active               bit             not null,
  pseudoID             int,
  description          nvarchar(1000),
  englishLevelID       int             not null,
  skillID              int             not null,
  surcharge            float           not null,
  hourlyWage           float           not null,
  hours                float           not null,
  hourRange            int,
  days                 int             not null,
  qualityOfWork        int             not null,
  followDirections     int             not null,
  attitude             int             not null,
  reliability          int             not null,
  transportProgram     int             not null,
  comments             nvarchar(max),
  datecreated          datetime        not null,
  dateupdated          datetime        not null,
  Createdby            nvarchar(30),
  Updatedby            nvarchar(30),
  workerRating         int,
  workerRatingComments nvarchar(500),
  weightLifted         bit,
  skillEN              nvarchar(max),
  skillES              nvarchar(max),
  fullWAID             nvarchar(max),
  minEarnings          float not null,--default 0 not null,
  maxEarnings          float not null --default 0 not null
) WITH (
  DATA_SOURCE = pomonaBackup
)
-- DROP EXTERNAL TABLE WorkAssignments2018

--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--
--

SET IDENTITY_INSERT dbo.Employers ON
--GO
INSERT INTO dbo.Employers (
 ID
,active
,onlineSource
,returnCustomer
,receiveUpdates
,business
,name
,address1
,address2
,city
,state
,phone
,cellphone
,zipcode
,email
,referredby
,referredbyOther
,blogparticipate
,notes
,datecreated
,dateupdated
,Createdby
,Updatedby
,businessname
,licenseplate
,driverslicense
,onlineSigninID
,isOnlineProfileComplete
,fax
)
  SELECT --*
 ID
,active
,onlineSource
,returnCustomer
,receiveUpdates
,business
,name
,address1
,address2
,city
,state
,phone
,cellphone
,zipcode
,email
,referredby
,referredbyOther
,blogparticipate
,notes
,datecreated
,dateupdated
,Createdby
,Updatedby
,businessname
,licenseplate
,driverslicense
,onlineSigninID
,isOnlineProfileComplete
,fax
    FROM Employers2018
    WHERE ID = 2002
GO
SET IDENTITY_INSERT dbo.Employers OFF
GO

SET IDENTITY_INSERT dbo.WorkOrders ON
GO
INSERT INTO dbo.WorkOrders (
 ID
,EmployerID
,onlineSource
,paperOrderNum
,waPseudoIDCounter
,contactName
,status
,workSiteAddress1
,workSiteAddress2
,city
,state
,phone
,zipcode
,typeOfWorkID
,englishRequired
,englishRequiredNote
,lunchSupplied
,permanentPlacement
,transportMethodID
,transportFee
,transportFeeExtra
,description
,dateTimeofWork
,timeFlexible
,datecreated
,dateupdated
,Createdby
,Updatedby
,transportTransactType
,transportTransactID
,additionalNotes
,disclosureAgreement
,paypalFee
,paypalToken
,paypalPayerId
,paypalErrors
,paypalTransactID
,statusEN
,statusES
,transportMethodEN
,transportMethodES
,timeZoneOffset
)
  SELECT --*
 ID
,EmployerID
,onlineSource
,paperOrderNum
,waPseudoIDCounter
,contactName
,status
,workSiteAddress1
,workSiteAddress2
,city
,state
,phone
,zipcode
,typeOfWorkID
,englishRequired
,englishRequiredNote
,lunchSupplied
,permanentPlacement
,transportMethodID
,transportFee
,transportFeeExtra
,description
,dateTimeofWork
,timeFlexible
,datecreated
,dateupdated
,Createdby
,Updatedby
,transportTransactType
,transportTransactID
,additionalNotes
,disclosureAgreement
,paypalFee
,paypalToken
,paypalPayerId
,paypalErrors
,paypalTransactID
,statusEN
,statusES
,transportMethodEN
,transportMethodES
,timeZoneOffset
    FROM WorkOrders2018
    WHERE EmployerID = 2002
GO
SET IDENTITY_INSERT dbo.WorkOrders OFF
GO

SET IDENTITY_INSERT dbo.WorkAssignments ON
GO
INSERT INTO dbo.WorkAssignments (
 ID
,workerAssignedID
,workOrderID
,workerSigninID
,active
,pseudoID
,description
,englishLevelID
,skillID
,surcharge
,hourlyWage
,hours
,hourRange
,days
,qualityOfWork
,followDirections
,attitude
,reliability
,transportProgram
,comments
,datecreated
,dateupdated
,Createdby
,Updatedby
,workerRating
,workerRatingComments
,weightLifted
,skillEN
,skillES
,fullWAID
,minEarnings
,maxEarnings
)
  SELECT --wa.*
 wa.ID
,wa.workerAssignedID
,wa.workOrderID
,wa.workerSigninID
,wa.active
,wa.pseudoID
,wa.description
,wa.englishLevelID
,wa.skillID
,wa.surcharge
,wa.hourlyWage
,wa.hours
,wa.hourRange
,wa.days
,wa.qualityOfWork
,wa.followDirections
,wa.attitude
,wa.reliability
,wa.transportProgram
,wa.comments
,wa.datecreated
,wa.dateupdated
,wa.Createdby
,wa.Updatedby
,wa.workerRating
,wa.workerRatingComments
,wa.weightLifted
,wa.skillEN
,wa.skillES
,wa.fullWAID
,wa.minEarnings
,wa.maxEarnings
    FROM dbo.WorkAssignments2018 wa
    JOIN dbo.WorkOrders2018 wo
      ON wa.workOrderID = wo.ID
    WHERE wo.EmployerID = 2002
GO
SET IDENTITY_INSERT dbo.WorkAssignments OFF
GO

SELECT *
  FROM dbo.Employers e
  JOIN dbo.WorkOrders wo ON e.ID = wo.EmployerID
  JOIN dbo.WorkAssignments wa ON wo.ID = wa.workOrderID
  WHERE e.ID = 2002

DROP EXTERNAL TABLE WorkAssignments2018
DROP EXTERNAL TABLE WorkOrders2018
DROP EXTERNAL TABLE Employers2018
DROP EXTERNAL DATA SOURCE pomonaBackup
DROP DATABASE SCOPED CREDENTIAL remoteCredential
DROP MASTER KEY

