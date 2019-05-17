USE [macheteStageProd]
GO
/****** Object:  View [db_datareader].[2011_count_of_first_signins_by_month]    Script Date: 09/13/2011 11:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [db_datareader].[2011_count_of_first_signins_by_month]
AS
SELECT     CONVERT(VARCHAR(7), firstSignin, 111) AS yearmonth, COUNT(dwccardnum) AS uniquecards
FROM         (SELECT     dwccardnum, MIN(dateforsignin) AS firstSignin
                       FROM          dbo.WorkerSignins
                       WHERE      (DATEPART(yy, dateforsignin) = 2011)
                       GROUP BY dwccardnum) AS foo
GROUP BY CONVERT(VARCHAR(7), firstSignin, 111)


GO
/****** Object:  View [db_datareader].[average_of_wage_by_month]    Script Date: 09/13/2011 11:07:16 ******/

CREATE VIEW [db_datareader].[average_of_wage_by_month]
AS
SELECT     TOP (100) PERCENT CONVERT(VARCHAR(7), wo.dateTimeofWork, 111) AS yearmonth, '$' + CONVERT(varchar, CONVERT(money, 
                      SUM(wa.days * wa.hours * wa.hourlyWage) / SUM(wa.days * wa.hours)), 1) AS dispatchedWageAvg
FROM         dbo.WorkAssignments AS wa INNER JOIN
                      dbo.WorkOrders AS wo ON wa.workOrderID = wo.ID
WHERE     (wo.status = 44)
GROUP BY CONVERT(VARCHAR(7), wo.dateTimeofWork, 111)
ORDER BY yearmonth DESC
go

CREATE VIEW [db_datareader].[average_of_wage_by_year]
AS
SELECT     TOP (100) PERCENT DATEPART(yy, wo.dateTimeofWork) AS year, '$' + CONVERT(varchar, CONVERT(money, SUM(wa.days * wa.hours * wa.hourlyWage) 
                      / SUM(wa.days * wa.hours)), 1) AS dispatchedWageAvg
FROM         dbo.WorkAssignments AS wa INNER JOIN
                      dbo.WorkOrders AS wo ON wa.workOrderID = wo.ID
WHERE     (wo.status = 44)
GROUP BY DATEPART(yy, wo.dateTimeofWork)
ORDER BY year DESC
GO

CREATE VIEW [db_datareader].[count_of_dispatched_by_month]
AS
SELECT     TOP (100) PERCENT CONVERT(VARCHAR(7), wo.dateTimeofWork, 111) AS yearmonth, COUNT(wa.ID) AS dispatchedCount
FROM         dbo.WorkAssignments AS wa INNER JOIN
                      dbo.WorkOrders AS wo ON wa.workOrderID = wo.ID
WHERE     (wo.status = 44)
GROUP BY CONVERT(VARCHAR(7), wo.dateTimeofWork, 111)
ORDER BY yearmonth DESC
GO

CREATE VIEW [db_datareader].[count_of_unique_workers_by_month]
AS
SELECT     TOP (100) PERCENT yearmonth, COUNT(dwccardnum) AS countofcards
FROM         (SELECT     dwccardnum, CONVERT(VARCHAR(7), dateforsignin, 111) AS yearmonth
                       FROM          dbo.WorkerSignins
                       GROUP BY dwccardnum, CONVERT(VARCHAR(7), dateforsignin, 111)) AS foo
GROUP BY yearmonth
ORDER BY yearmonth
GO

CREATE VIEW [db_datareader].[employers_EN]
AS
SELECT     ID, active, name, address1, address2, city, state, phone, cellphone, zipcode, email,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = dbo.Employers.referredby)) AS referredby, referredbyOther, datecreated, dateupdated, Createdby, Updatedby
FROM         dbo.Employers

GO

CREATE VIEW [db_datareader].[united_way_demographics]
AS
SELECT     dbo.Workers.dwccardnum, CASE dbo.persons.gender WHEN 38 THEN 'DWC' WHEN 39 THEN 'HHH' ELSE 'Unk' END AS secondidentifier, 
                      '00000' + dbo.Workers.dwccardnum AS frid, dbo.Persons.city, dbo.Persons.zipcode, CASE isnull(dbo.persons.address1, 'NULLISSTUPID') 
                      WHEN 'NULLISSTUPID' THEN 1 ELSE 0 END AS homeless, CASE isnull(dbo.persons.zipcode, 'NULL') WHEN 'NULL' THEN 1 ELSE 0 END AS zipunknown, 
                      CASE dbo.Persons.gender WHEN 38 THEN '2' WHEN 39 THEN '1' WHEN 40 THEN '3' END AS gender, CASE raceid WHEN 1 THEN 1 ELSE 0 END AS AfricanAmerican, 
                      CASE raceid WHEN 2 THEN 1 ELSE 0 END AS Asian, CASE raceid WHEN 4 THEN 1 ELSE 0 END AS PacificHawaiian, 
                      CASE raceid WHEN 5 THEN 1 ELSE 0 END AS Hispanic, CASE raceid WHEN 6 THEN 1 ELSE 0 END AS NativeAmerican, 
                      CASE raceid WHEN 3 THEN 1 ELSE 0 END AS WhiteCaucasian, CASE raceid WHEN 7 THEN 1 ELSE 0 END AS Other, '0' AS raceunknown, 
                      CASE englishlevelID WHEN 0 THEN 2 WHEN 1 THEN 2 WHEN 2 THEN 2 WHEN 3 THEN 1 WHEN 4 THEN 1 ELSE 0 END AS limitedenglish, 
                      CASE recentarrival WHEN 0 THEN 1 WHEN 1 THEN 2 ELSE 0 END AS newarrival, CASE disabled WHEN 0 THEN 1 WHEN 1 THEN 2 ELSE 0 END AS disability, 
                      DATEDIFF(year, dbo.Workers.dateOfBirth, GETDATE()) AS age, '0' AS ageunknown, 
                      CASE dbo.workers.maritalstatus WHEN 34 THEN 2 ELSE 1 END + dbo.Workers.numofchildren AS hhsize, dbo.Workers.numofchildren, 
                      CASE WHEN dbo.workers.maritalstatus <> 34 AND dbo.workers.numofchildren > 0 THEN 1 ELSE 0 END AS singleparent, '0' AS HHunknown, 
                      CASE WHEN dbo.workers.incomeID = 17 THEN 1 ELSE 2 END AS income, '1' AS nr02505, '0' AS nr02501, '0' AS xxxx1, '0' AS xxxx2
FROM         dbo.Workers INNER JOIN
                      dbo.Persons ON dbo.Workers.ID = dbo.Persons.ID
GO

CREATE VIEW [db_datareader].[workAssignments_status_summary_EN]
AS
SELECT     TOP (100) PERCENT startdate, SUM(CASE status WHEN 43 THEN countof ELSE '' END) AS [Pending Assignments], 
                      SUM(CASE status WHEN 42 THEN countof ELSE '' END) AS [Active Assignments], SUM(CASE status WHEN 44 THEN countof ELSE '' END) 
                      AS [Completed Assignments], SUM(CASE status WHEN 45 THEN countof ELSE '' END) AS [Cancelled Assignments], 
                      SUM(CASE status WHEN 46 THEN countof ELSE '' END) AS [Expired Assignments]
FROM         (SELECT     startdate, status, COUNT(ID) AS countof
                       FROM          (SELECT     CONVERT(char(10), wo.dateTimeofWork, 126) AS startdate, was.ID, wo.status
                                               FROM          dbo.WorkAssignments AS was INNER JOIN
                                                                      dbo.WorkOrders AS wo ON wo.ID = was.workOrderID) AS waa
                       GROUP BY startdate, status) AS summed
GROUP BY startdate
ORDER BY startdate DESC
GO

CREATE VIEW [db_datareader].[workers_critical_attributes_EN]
AS
SELECT     w.dwccardnum AS memberID, CASE w.active WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS activeworker, 
                      w.dateOfMembership AS [date of membership], w.memberexpirationdate AS [member expiration date], p.firstname1, p.firstname2, p.lastname1, p.lastname2, 
                      w.englishlevelID AS [english level],
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = p.gender)) AS gender,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = w.typeOfWorkID)) AS [Worker program],
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = w.skill1)) AS Skill1,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = w.skill2)) AS Skill2,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = w.skill3)) AS Skill3, w.datecreated, w.Createdby, w.dateupdated, w.Updatedby
FROM         dbo.Persons AS p INNER JOIN
                      dbo.Workers AS w ON w.ID = p.ID

GO

CREATE VIEW [db_datareader].[workers_EN]
AS
SELECT     wo.ID, p.firstname1, p.firstname2, p.lastname1, p.lastname2, p.address1, p.address2, p.city, p.state, CASE WHEN p.zipcode IS NULL 
                      THEN 'unknown' ELSE p.zipcode END AS zipcode, p.phone,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = p.gender)) AS Gender, wo.dateOfMembership, wo.dateOfBirth, 
                      CASE p.[active] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS activeworker,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.RaceID)) AS Race, wo.raceother, wo.height, wo.weight, wo.englishlevelID AS englishlevel, 
                      CASE [recentarrival] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS recentarrival, wo.dateinUSA, wo.dateinseattle, 
                      CASE [disabled] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS disabled, wo.disabilitydesc,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.maritalstatus)) AS maritalstatus, CASE [livewithchildren] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS livewithchildren, 
                      wo.numofchildren,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.incomeID)) AS incomeID, CASE [livealone] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS livealone, wo.emcontUSAname, 
                      wo.emcontUSArelation, wo.emcontUSAphone, wo.dwccardnum,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.neighborhoodID)) AS neighborhood, CASE [immigrantrefugee] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS immigrantrefugee,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.countryoforiginID)) AS countryoforigin, wo.emcontoriginname, wo.emcontoriginrelation, wo.emcontoriginphone, wo.memberexpirationdate, 
                      CASE [driverslicense] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS driverslicense, wo.licenseexpirationdate, 
                      CASE [carinsurance] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS carinsurance, wo.insuranceexpiration, wo.ImageID, wo.skill1, wo.skill2, 
                      wo.skill3, wo.datecreated, wo.dateupdated, wo.Createdby, wo.Updatedby
FROM         dbo.Workers AS wo INNER JOIN
                      dbo.Persons AS p ON wo.ID = p.ID
GO

CREATE VIEW [db_datareader].[workOrders_EN]
AS
SELECT     wo.ID, wo.EmployerID, dbo.Employers.name AS EmployerName, wo.contactName,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.status)) AS Status, wo.workSiteAddress1, wo.workSiteAddress2, wo.city, wo.state, wo.phone, wo.zipcode,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.typeOfWorkID)) AS TypeOfWorkOrder, CASE [englishRequired] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS EnglishRequired,
                       wo.englishRequiredNote, CASE [lunchSupplied] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS LunchSupplied, 
                      CASE [permanentPlacement] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS permanentPlacement,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.transportMethodID)) AS TransportMethod, wo.transportFee, wo.transportFeeExtra, wo.description, wo.dateTimeofWork, 
                      CASE [timeFlexible] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS FlexibleStateTime, wo.datecreated, wo.dateupdated, wo.Createdby, 
                      wo.Updatedby, wo.paperOrderNum, wo.waPseudoIDCounter AS AssignmentCount
FROM         dbo.WorkOrders AS wo INNER JOIN
                      dbo.Employers ON wo.EmployerID = dbo.Employers.ID
GO

CREATE VIEW [db_datareader].[workOrders_status_summary_EN]
AS
SELECT     TOP (100) PERCENT startdate, SUM(CASE status WHEN 43 THEN countof ELSE '' END) AS [Pending Orders], SUM(CASE status WHEN 42 THEN countof ELSE '' END) 
                      AS [Active Orders], SUM(CASE status WHEN 44 THEN countof ELSE '' END) AS [Completed Orders], SUM(CASE status WHEN 45 THEN countof ELSE '' END) 
                      AS [Cancelled Orders], SUM(CASE status WHEN 46 THEN countof ELSE '' END) AS [Expired Orders]
FROM         (SELECT     startdate, status, COUNT(ID) AS countof
                       FROM          (SELECT     CONVERT(char(10), dateTimeofWork, 126) AS startdate, ID, status
                                               FROM          dbo.WorkOrders) AS wo
                       GROUP BY startdate, status) AS summed
GROUP BY startdate
ORDER BY startdate DESC
GO

CREATE VIEW [db_datareader].[count_of_unique_employed_workers_by_month]
AS
SELECT     yearmonth, COUNT(workerAssignedID) AS uniqueEmployments
FROM         (SELECT     CONVERT(VARCHAR(7), wo.dateTimeofWork, 111) AS yearmonth, wa.workerAssignedID
                       FROM          dbo.WorkAssignments AS wa INNER JOIN
                                              dbo.WorkOrders AS wo ON wa.workOrderID = wo.ID
                       WHERE      (wo.status = 44) AND (wo.dateTimeofWork < GETDATE()) AND (wa.workerAssignedID IS NOT NULL)
                       GROUP BY CONVERT(VARCHAR(7), wo.dateTimeofWork, 111), wa.workerAssignedID) AS foo
GROUP BY yearmonth

GO

CREATE VIEW [db_datareader].[status_summary_combined_EN]
AS
SELECT     TOP (100) PERCENT db_datareader.workOrders_status_summary_EN.startdate, CASE datepart(dw, db_datareader.workOrders_status_summary_EN.startdate) 
                      WHEN 1 THEN 'Sunday' WHEN 2 THEN 'Monday' WHEN 3 THEN 'Tuesday' WHEN 4 THEN 'Wednesday' WHEN 5 THEN 'Thursday' WHEN 6 THEN 'Friday' WHEN 7 THEN
                       'Saturday' END AS Expr1, db_datareader.workOrders_status_summary_EN.[Pending Orders], 
                      db_datareader.workAssignments_status_summary_EN.[Pending Assignments], db_datareader.workOrders_status_summary_EN.[Active Orders], 
                      db_datareader.workAssignments_status_summary_EN.[Active Assignments], db_datareader.workOrders_status_summary_EN.[Completed Orders], 
                      db_datareader.workAssignments_status_summary_EN.[Completed Assignments], db_datareader.workOrders_status_summary_EN.[Cancelled Orders], 
                      db_datareader.workAssignments_status_summary_EN.[Cancelled Assignments], db_datareader.workOrders_status_summary_EN.[Expired Orders], 
                      db_datareader.workAssignments_status_summary_EN.[Expired Assignments]
FROM         db_datareader.workOrders_status_summary_EN INNER JOIN
                      db_datareader.workAssignments_status_summary_EN ON 
                      db_datareader.workOrders_status_summary_EN.startdate = db_datareader.workAssignments_status_summary_EN.startdate
ORDER BY db_datareader.workOrders_status_summary_EN.startdate DESC

GO


CREATE VIEW [db_datareader].[2011_count_of_unique_employments_divided_by_month]
AS
SELECT     CONVERT(VARCHAR(7), datetimeofwork, 111) AS yearmonth, COUNT(memberid) AS uniqueEmployment
FROM         (SELECT     w.dwccardnum AS memberid, MIN(wo.dateTimeofWork) AS datetimeofwork
                       FROM          dbo.WorkAssignments AS wa INNER JOIN
                                              dbo.WorkOrders AS wo ON wo.ID = wa.workOrderID INNER JOIN
                                              dbo.Workers AS w ON w.ID = wa.workerAssignedID
                       WHERE      (DATEPART(yy, wo.dateTimeofWork) = 2011)
                       GROUP BY w.dwccardnum) AS foo
GROUP BY CONVERT(VARCHAR(7), datetimeofwork, 111)

GO


CREATE VIEW [db_datareader].[workers_signins_past_membership_expiration]
AS
SELECT     TOP (100) PERCENT dbo.WorkerSignins.dwccardnum, p.firstname1, p.firstname2, p.lastname1, p.lastname2, MAX(dbo.WorkerSignins.dateforsignin) AS latestsignin, 
                      COUNT(w.dateOfMembership) AS countofsignins, MAX(w.memberexpirationdate) AS memberexpirationdate, DATEDIFF(dy, MAX(w.memberexpirationdate), 
                      MAX(dbo.WorkerSignins.dateforsignin)) AS daysdelequent
FROM         dbo.Workers AS w INNER JOIN
                      dbo.WorkerSignins ON w.ID = dbo.WorkerSignins.WorkerID AND w.memberexpirationdate < dbo.WorkerSignins.dateforsignin INNER JOIN
                      dbo.Persons AS p ON w.ID = p.ID
GROUP BY dbo.WorkerSignins.dwccardnum, p.firstname1, p.firstname2, p.lastname1, p.lastname2

GO