using Machete.Data.Helpers;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data
{
    public static class MacheteReportDefinitions
    {

        public static List<ReportDefinition> cache { get { return _cache; } }

        private static List<ReportDefinition> _cache = new List<ReportDefinition>
        {
            #region Report definitions
            // DispatchesByJob
            new ReportDefinition {
                name = "DispatchesByJob",
                commonName = "Dispatches by job",
                category = "Dispatches",
                description = "The number of completed dispatches, grouped by job (skill ID)",
                sqlquery = @"SELECT
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), min(wa.skillid)) as id,
lskill.text_en  AS label,
count(lskill.text_en) value
FROM [dbo].WorkAssignments as WA 
join [dbo].lookups as lskill on (wa.skillid = lskill.id)
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@startDate)
and lstatus.text_en = 'Completed'
group by lskill.text_en"
            },
            // DispatchesByMonth
            new ReportDefinition {
                name = "DispatchesByMonth",
                commonName = "Dispatches by month",
                description = "The number of completed dispatches, grouped by month",
                category = "Dispatches",
                sqlquery = @"SELECT
convert(varchar(23), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), month(min(wo.datetimeofwork))) as id,
convert(varchar(7), min(wo.datetimeofwork), 126)  AS label,
count(*) value
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @startDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'
and wa.workerassignedid is not null
group by month(wo.datetimeofwork)"
            },
            // WorkersByIncome
            new ReportDefinition
            {
                name = "WorkersByIncome",
                commonName = "Workers by income",
                description = "A count of workers by income level who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery = 
@"select L.text_EN as label, count(*) as value
FROM (
  select W.ID, W.incomeID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.incomeID
) as WW
JOIN dbo.Lookups L ON L.ID = WW.incomeID
group by L.text_EN

union 

select 'NULL' as label, count(*) as value
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   and W.incomeID is null
   group by W.ID
) as WWW"
            },
            // WorkersByDisability
            new ReportDefinition
            {
                name = "WorkersByDisability",
                commonName = "Workers by disability",
                description = "A count of workers by disability status who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(disabled) as id,
disabled as label, 
count(*) as value
FROM (
  select W.ID, 
  CASE 
	WHEN W.disabled = 1 then 'yes'
	when W.disabled = 0 then 'no'
	when W.disabled is null then 'NULL'
  END as disabled
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.disabled
) as WW
group by disabled"
            },
            // WorkersByLivingSituation
            new ReportDefinition
            {
                name = "WorkersByLivingSituation",
                commonName = "Workers by living situation",
                description = "A count of workers by homeless status who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(homeless) as id,
homeless as label, 
count(*) as value
FROM (
  select W.ID, 
  CASE 
	WHEN W.homeless = 1 then 'yes'
	when W.homeless = 0 then 'no'
	when W.homeless is null then 'NULL'
  END as homeless
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.homeless
) as WW
group by homeless"
            },
            // WorkersByHouseholdComposition
            new ReportDefinition
            {
                name = "WorkersByHouseholdComposition",
                commonName = "Workers by household composition",
                description = "A count of workers by household composition who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
id + '-' + myid as id, label, count(*) as value
FROM (
  select convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) as id,
  CASE 
	WHEN W.livewithchildren = 1 then 'Households with minors - details unknown'
	when W.livealone = 1 then 'Single person household - details unknown'
	when (W.livealone = 0 or w.livealone is null) and (W.livewithchildren = 0 or w.livewithchildren is null) then 'Household composition unknown'
  END as label,
  CASE 
	WHEN W.livewithchildren = 1 then '1'
	when W.livealone = 1 then '2'
	when (W.livealone = 0 or w.livealone is null) and (W.livewithchildren = 0 or w.livewithchildren is null) then '3'
  END as myid
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.livewithchildren, W.livealone
) as WW
group by ID, label, myid"
            },
            // WorkersByArrivalStatus
            new ReportDefinition
            {
                name = "WorkersByArrivalStatus",
                commonName = "Workers by arrival status",
                description = "A count of workers by immigrant/refugee/new arrival status who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(immigrantrefugee) as id,
immigrantrefugee as label, 
count(*) as value
FROM (
  select W.ID, 
  CASE 
	WHEN W.immigrantrefugee = 1 then 'yes'
	when W.immigrantrefugee = 0 then 'no'
	when W.immigrantrefugee is null then 'NULL'
  END as immigrantrefugee
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.immigrantrefugee
) as WW
group by immigrantrefugee"
            },
            // WorkersByLimitedEnglish
            new ReportDefinition
            {
                name = "WorkersByLimitedEnglish",
                commonName = "Workers by limited English status",
                description = "A count of workers by limited english ability who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(immigrantrefugee) as id,
immigrantrefugee as label, 
count(*) as value
FROM (
  select W.ID, 
  CASE 
	WHEN W.immigrantrefugee = 1 then 'yes'
	when W.immigrantrefugee = 0 then 'no'
	when W.immigrantrefugee is null then 'NULL'
  END as immigrantrefugee
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.immigrantrefugee
) as WW
group by immigrantrefugee"
            },
            // WorkersByZipcode
            new ReportDefinition
            {
                name = "WorkersByZipcode",
                commonName = "Workers by zipcode",
                description = "A count of workers by zipcodey who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(zipcode) as id,
zipcode as label, 
count(*) as value
FROM (
  select W.ID, w.zipcode
  from Persons W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.zipcode
) as WW
group by zipcode"
            },
            // WorkersByLatinoStatus
            new ReportDefinition
            {
                name = "WorkersByLatinoStatus",
                commonName = "Workers by latino status",
                description = "A count of workers by ethnicity (Spanish/Hispanic/Latino) who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + min(raceID) as id,
raceID as label, 
count(*) as value
FROM (
  select W.ID, 
  CASE 
	WHEN W.raceID = 5 then 'Spanish/Hispanic/Latino'
	when W.raceID <> 5 then 'Not Spanish/Hispanic/Latino'
	when W.raceID is null then 'NULL'
  END as raceID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.raceID
) as WW
group by raceID"
            },
            // WorkersByEthnicGroup
            new ReportDefinition
            {
                name = "WorkersByEthnicGroup",
                commonName = "Workersr by ethnic group",
                description = "A count of workers by ethnic group who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), min(WW.raceID)) as id,
L.text_EN as label, 
count(*) as value
FROM (
  select W.ID, W.raceID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.raceID
) as WW
JOIN dbo.Lookups L ON L.ID = WW.raceID
group by L.text_EN

union 

select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-NULL' as id,
'NULL' as label, 
count(*) as value
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   and W.raceID is null
   group by W.ID
) as WWW"
            },
            // WorkersByGender
            new ReportDefinition
            {
                name = "WorkersByGender",
                commonName = "Workers by gender",
                description = "A count of workers by gender who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), min(WW.gender)) as id,
L.text_EN as label, 
count(*) as value
FROM (
  select W.ID, W.gender
  from persons W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.gender
) as WW
JOIN dbo.Lookups L ON L.ID = WW.gender
group by L.text_EN

union 

select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-NULL' as id,
'NULL' as label, 
count(*) as value
from (
   select W.ID, min(dateforsignin) firstsignin
   from persons W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   and W.gender is null
   group by W.ID
) as WWW
"
            },
            // WorkersByAgeGroupBase10
            new ReportDefinition 
            {
                name = "WorkersByAgeGroupBase10",
                commonName = "Workers by age group, 10 year groupings",
                description = 
@"The count of workers who have signed in at least one during the search period,
grouped by the age, in 10-year groupings",
                category = "Demographics",
                sqlquery =
@"
with demos_age (age_range, ordinal)
as 
(
	SELECT
	CASE
		WHEN age < 20 THEN 'Under_20'
		WHEN age BETWEEN 20 and 29 THEN '20-29'
		WHEN age BETWEEN 30 and 39 THEN '30-39'
		WHEN age BETWEEN 40 and 49 THEN '40-49'
		WHEN age BETWEEN 50 and 59 THEN '50-59'
		WHEN age BETWEEN 60 and 69 THEN '60-69'
		WHEN age BETWEEN 70 and 79 THEN '70-79'
		WHEN age >= 80 THEN 'Over_80'
		WHEN age IS NULL THEN 'NULL'
	END as age_range,
		CASE
		WHEN age < 20 THEN 1
		WHEN age BETWEEN 20 and 29 THEN 2
		WHEN age BETWEEN 30 and 39 THEN 3
		WHEN age BETWEEN 40 and 49 THEN 4
		WHEN age BETWEEN 50 and 59 THEN 5
		WHEN age BETWEEN 60 and 69 THEN 6
		WHEN age BETWEEN 70 and 79 THEN 7
		WHEN age >= 80 THEN 8
		WHEN age IS NULL THEN 9
	END as ordinal
	FROM (
		select w.id, w.age, w.dateofbirth
		from (
			select ID,(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),dateOfBirth,112))/10000 AS age, dateofbirth
			from Workers
		) as W
		join workersignins wsi on wsi.workerID = W.ID
		WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
		group by w.id, w.age, w.dateofbirth
	) as a
)
select 
	convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(10), min(age_range)) as id,	
	age_range as label, 
	count(*) as value
from demos_age 
group by age_range, ordinal
order by ordinal
"
            },
            // WorkersByAgeGroupUnitedWay'
            new ReportDefinition
            {
                name = "WorkersByAgeGroupUnitedWay",
                commonName = "Workers by age group (United Way)",
                description =
@"The count of workers who have signed in at least one during the search period,
grouped by the age, in United Way's reporting groups",
                category = "Demographics",
                sqlquery =
@"
with demos_age (age_range, ordinal)
as 
(
	SELECT
	CASE
		WHEN age < 14 THEN 'Under_14'
		WHEN age BETWEEN 14 and 17 THEN '14-17'
		WHEN age BETWEEN 18 and 24 THEN '18-24'
		WHEN age BETWEEN 25 and 34 THEN '25-34'
		WHEN age BETWEEN 35 and 54 THEN '35-54'
		WHEN age BETWEEN 55 and 59 THEN '55-59'
		WHEN age BETWEEN 60 and 64 THEN '60-64'
		WHEN age BETWEEN 65 and 74 THEN '65-74'
		WHEN age BETWEEN 75 and 84 THEN '75-84'
		WHEN age >= 85 THEN 'Over_85'
		WHEN age IS NULL THEN 'NULL'
	END as age_range,
		CASE
		WHEN age < 14 THEN 1
		WHEN age BETWEEN 14 and 17 THEN 2
		WHEN age BETWEEN 18 and 24 THEN 3
		WHEN age BETWEEN 25 and 34 THEN 4
		WHEN age BETWEEN 35 and 54 THEN 5
		WHEN age BETWEEN 55 and 59 THEN 6
		WHEN age BETWEEN 60 and 64 THEN 7
		WHEN age BETWEEN 65 and 74 THEN 8
		WHEN age BETWEEN 75 and 84 THEN 9
		WHEN age >= 85 THEN 10
		WHEN age IS NULL THEN 11
	END as ordinal
	FROM (
		select w.id, w.age, w.dateofbirth
		from (
			select ID,(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),dateOfBirth,112))/10000 AS age, dateofbirth
			from Workers
		) as W
		join workersignins wsi on wsi.workerID = W.ID
		WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
		group by w.id, w.age, w.dateofbirth
	) as a
)
select 
	convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(10), min(age_range)) as id,	
	age_range as label, 
	count(*) as value
from demos_age 
group by age_range, ordinal
order by ordinal
"
            },
            new ReportDefinition
            {
                name = "TransportationFeeDetails",
                commonName = "Transportation Fees (detail list)",
                description = "A detailed list of transportation fees by date range",
                category = "Transportation",
                sqlquery = @"select
   WOs.dateTimeofWork AS workStartTime,
   WOs.paperOrderNum as wonum,
   text_EN as typeOfWork,
   name as employerName,
   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as wkrCount,
   transportFee as transFee,
   transportFeeExtra as exFee
from dbo.Lookups Ls
join dbo.WorkOrders WOs on Ls.ID = WOs.transportMethodID
join dbo.Employers Es on WOs.EmployerID = Es.ID
join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
where category like 'transportmethod' 
	and (transportFee + transportFeeExtra) > 0
    and WOs.dateTimeofWork > @startDate
	and WOs.dateTimeofWork < @endDate
	and WAs.workerAssignedID IS NOT NULL
group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra"
            },

            new ReportDefinition
            {
                name = "TransportationFeeMonthly",
                commonName = "Transportation Fees (monthly totals)",
                description = "The monthly totals of transportation fees by date range",
                category = "Transportation",
                sqlquery =
@"
select 
CONVERT(VARCHAR(7), workStartTime, 102) as Month,
count (wonum) as [Order count],
sum(wkrcount) as [Worker count],
sum(transfee) as [Transport fee],
sum(exfee) as [Extra fee]
from
(
	select
	   WOs.dateTimeofWork AS workStartTime,
	   WOs.paperOrderNum as wonum,
	   text_EN as typeOfWork,
	   name as employerName,
	   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as wkrCount,
	   transportFee as transFee,
	   transportFeeExtra as exFee
	from dbo.Lookups Ls
	join dbo.WorkOrders WOs on Ls.ID = WOs.transportMethodID
	join dbo.Employers Es on WOs.EmployerID = Es.ID
	join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
	join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
	where category like 'transportmethod' 
		and (transportFee + transportFeeExtra) > 0
		and WOs.dateTimeofWork > @startDate
		and WOs.dateTimeofWork < @endDate
		and WAs.workerAssignedID IS NOT NULL
	group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra
) as foo
group by CONVERT(VARCHAR(7), workStartTime, 102)"
            },

            new ReportDefinition
            {
                name = "ActivitiesESLAttendance",
                commonName = "ESL class attendance",
                description = @"A list of members' English class attendance by date range. Within the time period,
the report identifies if the member met the 12-hour or 24-hour threshold.",
                category = "Activities",
                sqlquery =
@"select 
    convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(10), min(member)) as id,	
	member as [Member ID], 
	sum(minutes) as [Total minutes],
	min([12hrTier]) as [12 hr completion],
	min([24hrTier]) as [24+ hr completion]
from
(
	SELECT COUNT(Acts.ID) as ActID,
	dwccardnum as Member,
	'English Class' as ClassName,
	SUM ( DATEDIFF( minute, dateStart, dateEnd )) as Minutes,
	CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 1440 THEN 1
	ELSE 0
	END AS [24hrTier],
	CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 720 AND SUM ( DATEDIFF ( minute, dateStart, dateEnd )) <= 1439 THEN 1
	ELSE 0
	END AS [12hrTier]
	from dbo.Activities Acts

	JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
	JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
	WHERE text_en LIKE 'English%'
	AND dateStart >= @startDate AND dateend <= @enddate

	GROUP BY dwccardnum
) as foo 
group by member"
            },

            new ReportDefinition
            {
                name = "DispatchesByMonthYearToYear",
                commonName = "Dispatches by month (pivot on year)",
                description = "The count of completed dispatches by month, pivoted on year",
                category = "Dispatches",
                sqlquery =
@"select year, 
	[1] as 'Jan', [2] as 'Feb', [3] as 'Mar', [4] as 'Apr',
	[5] as 'May', [6] as 'Jun', [7] as 'Jul', [8] as 'Aug',
	[9] as 'Sep', [10] as 'Oct', [11] as 'Nov', [12] as 'Dec'
from
(
	SELECT COUNT(WAs.[ID]) as jobDisp
	--,CONVERT(DECIMAL(10,2),AVG([hourlyWage])) AS avgWage
	,year([dateTimeOfWork]) AS year
	,month([dateTimeOfWork]) as month
	FROM [dbo].[WorkAssignments] WAs
	JOIN [dbo].[WorkOrders] WOs ON WAs.workOrderID = WOs.ID
	join [dbo].[Lookups] l on wos.status = l.id
	WHERE [workerAssignedID] IS NOT NULL
	AND [dateTimeOfWork] >= @startdate
	AND [dateTimeOfWork] <= @enddate
	and l.text_en = 'Completed'
	GROUP BY year([dateTimeOfWork]), month([dateTimeOfWork])
) as foo
PIVOT  
(  
sum (jobdisp)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt"
            },
            new ReportDefinition
            {
                name = "AverageWageByMonthYearToYear",
                commonName = "Average wage by month (pivot on year)",
                description = "The average hourly rate (wage) by month of completed jobs, pivoted on year",
                category = "Dispatches",
                sqlquery =
@"select year, 
	cast([1] as float) as 'Jan', 
	cast([2] as float) as 'Feb', 
	cast([3] as float) as 'Mar', 
	cast([4] as float) as 'Apr',
	cast([5] as float) as 'May', 
	cast([6] as float) as 'Jun', 
	cast([7] as float) as 'Jul', 
    cast([8] as float) as 'Aug',
	cast([9] as float) as 'Sep', 
	cast([10] as float) as 'Oct',
	cast([11] as float) as 'Nov', 
	cast([12] as float) as 'Dec'
from
(

	SELECT CONVERT(DECIMAL(10,2),AVG([hourlyWage])) AS avgWage
	,year([dateTimeOfWork]) AS year
	,month([dateTimeOfWork]) as month
	FROM [dbo].[WorkAssignments] WAs
	JOIN [dbo].[WorkOrders] WOs ON WAs.workOrderID = WOs.ID
	join [dbo].[Lookups] l on wos.status = l.id
	WHERE [workerAssignedID] IS NOT NULL
	AND [dateTimeOfWork] >= @startdate
	AND [dateTimeOfWork] <= @enddate
	and l.text_en = 'Completed'
	GROUP BY year([dateTimeOfWork]), month([dateTimeOfWork])
) as foo
PIVOT  
(  
sum (avgWage)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt "
            },
            new ReportDefinition
            {
                name = "DispatchesByProgramYearToYear",
                commonName = "Dispatches by month (pivot on year and program)",
                description = "The count of completed dispatches by month, pivoted on year and program",
                category = "Dispatches",
                sqlquery =
@"select year_program, 
	[1] as 'Jan', [2] as 'Feb', [3] as 'Mar', [4] as 'Apr',
	[5] as 'May', [6] as 'Jun', [7] as 'Jul', [8] as 'Aug',
	[9] as 'Sep', [10] as 'Oct', [11] as 'Nov', [12] as 'Dec'
from
(
	SELECT
	count(wa.workerAssignedID) JobCount
	,convert(varchar(4), year(wo.dateTimeofWork)) + '-' + convert(varchar(3), l.[key])  AS year_program
	,month(wo.dateTimeofWork) as month
	FROM
	WorkAssignments wa
	INNER JOIN Workers w ON wa.workerAssignedID = w.ID
	INNER JOIN WorkOrders wo ON wa.workOrderID = wo.ID
	inner join lookups l on w.typeOfWorkID = l.ID
	inner join lookups ll on wo.status = ll.id
	where [dateTimeOfWork] >= @startdate
	AND [dateTimeOfWork] <= @enddate
	and ll.text_en = 'Completed'
	group by l.Text_EN, l.[key], year(wo.dateTimeofWork), month(wo.dateTimeofWork)
) as foo
PIVOT  
(  
sum (JobCOunt)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt
"
            },

            //new ReportDefinition
            //{
            //    name = "",
            //    commonName = "",
            //    description = "",
            //    category = "",
            //    sqlquery = @""
            //},

            //new ReportDefinition
            //{
            //    name = "",
            //    commonName = "",
            //    description = "",
            //    category = "",
            //    sqlquery = @""
            //},

            //new ReportDefinition
            //{
            //    name = "",
            //    commonName = "",
            //    description = "",
            //    category = "",
            //    sqlquery = @""
            //},

            //new ReportDefinition
            //{
            //    name = "",
            //    commonName = "",
            //    description = "",
            //    category = "",
            //    sqlquery = @""
            //},

            //new ReportDefinition
            //{
            //    name = "",
            //    commonName = "",
            //    description = "",
            //    category = "",
            //    sqlquery = @""
            //},

            #endregion  
        };
        public static void Initialize(MacheteContext context)
        {
            _cache.ForEach(u => {
                try
                {
                    context.ReportDefinitions.First(a => a.name == u.name);
                }
                catch
                {
                    u.datecreated = DateTime.Now;
                    u.dateupdated = DateTime.Now;
                    u.createdby = "Init T. Script";
                    u.updatedby = "Init T. Script";
                    u.columnsJson = SqlServerUtils.getColumnJson(context, u.sqlquery);
                    context.ReportDefinitions.Add(u);
                }
            });
            context.Commit();
        }
    }

}
