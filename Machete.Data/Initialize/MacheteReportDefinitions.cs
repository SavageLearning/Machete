using Machete.Data.Helpers;
using Machete.Domain;
using Newtonsoft.Json;
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
                sqlquery =
@"SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByJob-' + convert(varchar(5), min(wa.skillid)) as id,
lskill.text_en  AS [Job type (skill)],
count(lskill.text_en) [Count]
FROM [dbo].WorkAssignments as WA 
join [dbo].lookups as lskill on (wa.skillid = lskill.id)
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@beginDate)
and lstatus.text_en = 'Completed'
group by lskill.text_en
union 
SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByJob-TOTAL' as id,
'Total' AS [Job type (skill)],
count(*) [Count]
FROM [dbo].WorkAssignments as WA 
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@beginDate)
and lstatus.text_en = 'Completed'"
            },
            // DispatchesByMonth
            new ReportDefinition {
                name = "DispatchesByMonth",
                commonName = "Dispatches by month",
                description = "The number of completed dispatches, grouped by month",
                category = "Dispatches",
                sqlquery =
@"SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonth-' + convert(varchar(5), month(min(wo.datetimeofwork))) as id,
convert(varchar(7), min(wo.datetimeofwork), 126)  AS [Month],
count(*) [Count]
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @beginDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'

group by month(wo.datetimeofwork)

union 

SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonth-TOTAL' as id,
'Total'  AS [Month],
count(*) [Count]
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @beginDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'"
            },
            // WorkersByIncome
            new ReportDefinition
            {
                name = "WorkersByIncome",
                commonName = "Workers by income",
                description = "A count of workers by income level who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByIncome-' + convert(varchar(3), min(l.id)) as id,
L.text_EN as [Income range], 
count(*) as [Count]
FROM (
  select W.ID, W.incomeID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.incomeID
) as WW
JOIN dbo.Lookups L ON L.ID = WW.incomeID
group by L.text_EN

union 

select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByIncome-NULL'  as id,

'NULL' as [Income range], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   and W.incomeID is null
   group by W.ID
) as WWW

union 

select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByIncome-TOTAL'  as id,

'Total' as [Income range], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByDisability-' + min(disabled) as id,
disabled as [Disabled?], 
count(*) as [Count]
FROM (
  select W.ID, 
  CASE 
	WHEN W.disabled = 1 then 'yes'
	when W.disabled = 0 then 'no'
	when W.disabled is null then 'NULL'
  END as disabled
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.disabled
) as WW
group by disabled
union 
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByDisability-TOTAL' as id,
'Total' as [Disabled?], 
count(distinct(w.id)) as [Count]
from Workers W
JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLivingSituation-' + min(homeless) as id,
homeless as [Homeless?], 
count(*) as [Count]
FROM (
  select W.ID, 
  CASE 
	WHEN W.homeless = 1 then 'yes'
	when W.homeless = 0 then 'no'
	when W.homeless is null then 'NULL'
  END as homeless
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.homeless
) as WW
group by homeless
union
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLivingSituation-TOTAL'  as id,
'Total' as [Homeless?], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByHouseholdComposition-' + myid as id, 
label as [Household], 
count(*) as [Count]
FROM 
(
  select  
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
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.livewithchildren, W.livealone
) as WW
group by label, myid
union 
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByHouseholdComposition-TOTAL'  as id,
'Total' as [Household], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
            },
            // WorkersByArrivalStatus
            new ReportDefinition
            {
                name = "WorkersByArrivalStatus",
                commonName = "Workers by arrival status",
                description = "A count of workers by immigrant/refugee/new arrival status who signed in looking for work at least once within the given time range.",
                category = "Demographics",
                sqlquery =
@"
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByArrivalStatus-' + min(immigrantrefugee) as id,
immigrantrefugee as [Immigrant / Refugee?], 
count(*) as [count]
FROM (
  select W.ID, 
  CASE 
	WHEN W.immigrantrefugee = 1 then 'yes'
	when W.immigrantrefugee = 0 then 'no'
	when W.immigrantrefugee is null then 'NULL'
  END as immigrantrefugee
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.immigrantrefugee
) as WW
group by immigrantrefugee
union 
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByArrivalStatus-TOTAL'  as id,
'Total' as [Immigrant / Refugee?], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLimitedEnglish-' + min(limitedEnglish) as id,
limitedEnglish as [Limited English?], 
count(*) as [Count]
FROM (
  select W.ID, 
  CASE 
	WHEN W.englishlevelID < 3 then 'yes'
	when W.englishlevelID >= 3 then 'no'
	when W.englishlevelID is null then 'NULL'
  END as limitedEnglish
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.Englishlevelid
) as WW
group by limitedEnglish

union 
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLimitedEnglish-TOTAL'  as id,
'Total' as [Limited English], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByZipcode-' + min(zipcode) as id,
zipcode as Zipcode, 
count(*) as [Count]
FROM (
  select W.ID, isnull(w.zipcode, 'NULL') as zipcode
  from Persons W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.zipcode
) as WW
group by zipcode
union
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByZipcode-TOTAL'  as id,
'Total' as [Zipcode], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLatinoStatus-' + min(raceID) as id,
raceID as [Latino status], 
count(*) as [Count]
FROM (
  select W.ID, 
  CASE 
	WHEN W.raceID = 5 then 'Spanish/Hispanic/Latino'
	when W.raceID <> 5 then 'Not Spanish/Hispanic/Latino'
	when W.raceID is null then 'NULL'
  END as raceID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.raceID
) as WW
group by raceID
union
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLatinoStatus-TOTAL'  as id,
'Total' as [Latino status], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByEthnicGroup-' + convert(varchar(5), min(WW.raceID)) as id,
L.text_EN as [Ethnic group], 
count(*) as [Count]
FROM (
  select W.ID, W.raceID
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.raceID
) as WW
JOIN dbo.Lookups L ON L.ID = WW.raceID
group by L.text_EN

union 

select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByEthnicGroup-NULL' as id,
'NULL' as [Ethnic group], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   and W.raceID is null
   group by W.ID
) as WWW

union
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByEthnicGroup-TOTAL'  as id,
'Total' as [Ethnic group], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
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
@"
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-' + convert(varchar(5), min(WW.gender)) as id,
L.text_EN as [Gender], 
count(*) as [Count]
FROM (
  select W.ID, W.gender
  from persons W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
  group by W.ID, W.gender
) as WW
JOIN dbo.Lookups L ON L.ID = WW.gender
group by L.text_EN

union 

select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-NULL' as id,
'NULL' as [Gender], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from persons W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   and W.gender is null
   group by W.ID
) as WWW

union
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-TOTAL'  as id,
'Total' as [Gender], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
   group by W.ID
) as WWW"
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
@"with demos_age (age_range, ordinal)
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
		WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
		group by w.id, w.age, w.dateofbirth
	) as a
)
select 
	convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByAgeGroupBase10-' + convert(varchar(10), min(age_range)) as id,	
	age_range as [Age range], 
	count(*) as [Count]
from demos_age 
group by age_range, ordinal
order by ordinal"
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
@"with demos_age (age_range, ordinal)
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
		WHERE dateforsignin >= @beginDate and dateforsignin <= @endDate
		group by w.id, w.age, w.dateofbirth
	) as a
)
select 
	convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByAgeGroupUnitedWay-' + convert(varchar(10), min(age_range)) as id,	
	age_range as [Age range], 
	count(*) as [Count]
from demos_age 
group by age_range, ordinal
order by ordinal"
            },
            // TransportationFeeDetails
            new ReportDefinition
            {
                name = "TransportationFeeDetails",
                commonName = "Transportation Fees (detail list)",
                description = "A detailed list of transportation fees by date range",
                category = "Transportation",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-TransportationFeeDetails-' + convert(varchar(6), min(WOs.paperOrderNum)) as id,
   WOs.dateTimeofWork AS [Work start time],
   WOs.paperOrderNum as [WO number],
   Ls.text_EN as [Transport type],
   name as [Employer name],
   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as [Wkr count],
   transportFee as [Transport fee],
   transportFeeExtra as [Extra fee]
from dbo.TransportProviders Ls
join dbo.WorkOrders WOs on Ls.ID = WOs.transportProviderID
join dbo.Employers Es on WOs.EmployerID = Es.ID
join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
where (transportFee + transportFeeExtra) > 0
    and WOs.dateTimeofWork > @beginDate
	and WOs.dateTimeofWork < @endDate
	and WAs.workerAssignedID IS NOT NULL
group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra"
            },
            // TransportationFeeMonthly
            new ReportDefinition
            {
                name = "TransportationFeeMonthly",
                commonName = "Transportation Fees (monthly totals)",
                description = "The monthly totals of transportation fees by date range",
                category = "Transportation",
                sqlquery =
@"
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-TransportationFeeMonthly-' + CONVERT(VARCHAR(7), workStartTime, 102)as id,
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
	   Ls.text_EN as typeOfWork,
	   name as employerName,
	   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as wkrCount,
	   transportFee as transFee,
	   transportFeeExtra as exFee
	from dbo.TransportProviders Ls
	join dbo.WorkOrders WOs on Ls.ID = WOs.transportProviderID
	join dbo.Employers Es on WOs.EmployerID = Es.ID
	join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
	join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
	where (transportFee + transportFeeExtra) > 0
		and WOs.dateTimeofWork > @beginDate
		and WOs.dateTimeofWork < @endDate
		and WAs.workerAssignedID IS NOT NULL
	group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra
) as foo
group by CONVERT(VARCHAR(7), workStartTime, 102)"
            },
            // ActivitiesESLAttendance
            new ReportDefinition
            {
                name = "ActivitiesESLAttendance",
                commonName = "ESL class attendance",
                description = @"A list of members' English class attendance by date range. Within the time period,
the report identifies if the member met the 12-hour or 24-hour threshold.",
                category = "Activities",
                sqlquery =
@"select 
    convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-' + convert(varchar(10), min(member)) as id,	
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
	WHERE 
	(
	  Ls.[key] = 'SomosVecinos' OR
	  Ls.[key] = 'EnglishClass1' OR
	  Ls.[key] = 'EnglishClass2' 
	)
	AND dateStart >= @beginDate AND dateend <= @enddate

	GROUP BY dwccardnum
) as foo 
group by member"
            },
            // DispatchesByMonthYearToYear
            new ReportDefinition
            {
                name = "DispatchesByMonthYearToYear",
                commonName = "Dispatches by month (pivot on year)",
                description = "The count of completed dispatches by month, pivoted on year",
                category = "Dispatches",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonthYearToYear-' + convert(varchar(4), [year])  as id,
year, 
	[1] as 'Jan', [2] as 'Feb', [3] as 'Mar', [4] as 'Apr',
	[5] as 'May', [6] as 'Jun', [7] as 'Jul', [8] as 'Aug',
	[9] as 'Sep', [10] as 'Oct', [11] as 'Nov', [12] as 'Dec'
from
(
	SELECT COUNT(WAs.[ID]) as jobDisp

	,year([dateTimeOfWork]) AS year
	,month([dateTimeOfWork]) as month
	FROM [dbo].[WorkAssignments] WAs
	JOIN [dbo].[WorkOrders] WOs ON WAs.workOrderID = WOs.ID
	join [dbo].[Lookups] l on wos.status = l.id
	WHERE [workerAssignedID] IS NOT NULL
	AND [dateTimeOfWork] >= @beginDate
	AND [dateTimeOfWork] <= @enddate
	and l.text_en = 'Completed'
	GROUP BY year([dateTimeOfWork]), month([dateTimeOfWork])
) as foo
PIVOT  
(  
sum (jobdisp)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt
 
ORDER BY pvt.year"
            },
            // AverageWageByMonthYearToYear
            new ReportDefinition
            {
                name = "AverageWageByMonthYearToYear",
                commonName = "Average wage by month (pivot on year)",
                description = "The average hourly rate (wage) by month of completed jobs, pivoted on year",
                category = "Dispatches",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-AverageWageByMonthYearToYear-' + convert(varchar(4), [year])  as id,
year, 
	cast(isnull([1],0) as float) as 'Jan', 
	cast(isnull([2],0) as float) as 'Feb', 
	cast(isnull([3],0) as float) as 'Mar', 
	cast(isnull([4],0) as float) as 'Apr',
	cast(isnull([5],0) as float) as 'May', 
	cast(isnull([6],0) as float) as 'Jun', 
	cast(isnull([7],0) as float) as 'Jul', 
    cast(isnull([8],0) as float) as 'Aug',
	cast(isnull([9],0) as float) as 'Sep', 
	cast(isnull([10],0) as float) as 'Oct',
	cast(isnull([11],0) as float) as 'Nov', 
	cast(isnull([12],0) as float) as 'Dec'
from
(
	SELECT CONVERT(DECIMAL(10,2),AVG([hourlyWage])) AS avgWage
	,year([dateTimeOfWork]) AS year
	,month([dateTimeOfWork]) as month
	FROM [dbo].[WorkAssignments] WAs
	JOIN [dbo].[WorkOrders] WOs ON WAs.workOrderID = WOs.ID
	join [dbo].[Lookups] l on wos.status = l.id
	WHERE [workerAssignedID] IS NOT NULL
	AND [dateTimeOfWork] >= @beginDate
	AND [dateTimeOfWork] <= @enddate
	and l.text_en = 'Completed'
	GROUP BY year([dateTimeOfWork]), month([dateTimeOfWork])
) as foo
PIVOT  
(  
sum (avgWage)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt 
"
            },
            // DispatchesByProgramYearToYear
            new ReportDefinition
            {
                name = "DispatchesByProgramYearToYear",
                commonName = "Dispatches by month (pivot on year and program)",
                description = "The count of completed dispatches by month, pivoted on year and program",
                category = "Dispatches",
                sqlquery =
@"select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByProgramYearToYear-' + year_program as id, 
year_program, 
	isnull([1],0) as 'Jan', 
	isnull([2],0) as 'Feb', 
	isnull([3],0) as 'Mar', 
	isnull([4],0) as 'Apr',
	isnull([5],0) as 'May', 
	isnull([6],0) as 'Jun', 
	isnull([7],0) as 'Jul', 
	isnull([8],0) as 'Aug',
	isnull([9],0) as 'Sep', 
	isnull([10],0) as 'Oct', 
	isnull([11],0) as 'Nov', 
	isnull([12],0) as 'Dec'
from
(
	SELECT
	count(wa.workerAssignedID) JobCount
	,convert(varchar(4), year(wo.dateTimeofWork)) + '-' + convert(varchar(3), isnull(l.[key], 'NUL'))  AS year_program
	,month(wo.dateTimeofWork) as month
	FROM
	WorkAssignments wa
	INNER JOIN Workers w ON wa.workerAssignedID = w.ID
	INNER JOIN WorkOrders wo ON wa.workOrderID = wo.ID
	inner join lookups l on w.typeOfWorkID = l.ID
	inner join lookups ll on wo.status = ll.id
	where [dateTimeOfWork] >= @beginDate
	AND [dateTimeOfWork] <= @enddate
	and ll.text_en = 'Completed'
	group by l.Text_EN, l.[key], year(wo.dateTimeofWork), month(wo.dateTimeofWork)
) as foo
PIVOT  
(  
sum (JobCOunt)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt"
            },
            // SeattleCityReport
            new ReportDefinition
            {
                name = "SeattleCityReport",
                commonName = "Seattle City report",
                description = "Casa Latina's monthly numbers for the City of Seattle",
                category = "site-specific",
                sqlquery =
@"select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-CityReport-NewEnrolls-' + convert(varchar(4), [year]) as id, 
    'Newly enrolled in program (within time range)' as label, 
    cast(year as int) as year, 
	cast([1] as int) as 'Jan', 
	cast([2] as int) as 'Feb', 
	cast([3] as int) as 'Mar', 
	cast([4] as int) as 'Apr',
	cast([5] as int) as 'May', 
	cast([6] as int) as 'Jun', 
	cast([7] as int) as 'Jul', 
	cast([8] as int) as 'Aug',
	cast([9] as int) as 'Sep', 
	cast([10] as int) as 'Oct', 
	cast([11] as int) as 'Nov', 
	cast([12] as int) as 'Dec'
from
(
	select min(year(signindate)) as year, min(month(signindate)) as month, cardnum
	from 
	(
		SELECT MIN(dateforsignin) AS signindate, dwccardnum as cardnum
		FROM dbo.WorkerSignins
		WHERE dateforsignin >= @beginDate AND
		dateforsignin < @EnDdate
		GROUP BY dwccardnum

		union 

		select min(dateforsignin) as singindate, dwccardnum as cardnum
		from activitysignins asi
		where dateforsignin >= @beginDate
		and dateforsignin < @enddate
		group by dwccardnum
	) 
	as result_set
	group by  cardnum
) as foo
PIVOT  
(  
count (cardnum)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt 

union 

select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-CityReport-FinEd-' + convert(varchar(4), [year]) as id, 
    'Counts of members who accessed finanacial literacy' as label, 
    cast(year as int) as year, 
	cast([1] as int) as 'Jan', 
	cast([2] as int) as 'Feb', 
	cast([3] as int) as 'Mar', 
	cast([4] as int) as 'Apr',
	cast([5] as int) as 'May', 
	cast([6] as int) as 'Jun', 
	cast([7] as int) as 'Jul', 
	cast([8] as int) as 'Aug',
	cast([9] as int) as 'Sep', 
	cast([10] as int) as 'Oct', 
	cast([11] as int) as 'Nov', 
	cast([12] as int) as 'Dec'
from
(
	select min(year(signindate)) as year, min(month(signindate)) as month, cardnum
	from 
	(
		SELECT ASIs.dwccardnum as cardnum, MIN(dateStart) as signindate
		FROM dbo.Activities Acts
		JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
		JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
		WHERE Ls.ID = 179 AND dateStart >= @beginDate AND dateStart <= @endDate

		GROUP BY ASIs.dwccardnum
	) 
	as result_set
	group by  cardnum
) as foo
PIVOT  
(  
count (cardnum)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt 

union 

select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-CityReport-Training-' + convert(varchar(4), [year]) as id, 
    'Counts of members who participated in job skill training or workshops' as label, 
    cast(year as int) as year, 
	cast([1] as int) as 'Jan', 
	cast([2] as int) as 'Feb', 
	cast([3] as int) as 'Mar', 
	cast([4] as int) as 'Apr',
	cast([5] as int) as 'May', 
	cast([6] as int) as 'Jun', 
	cast([7] as int) as 'Jul', 
	cast([8] as int) as 'Aug',
	cast([9] as int) as 'Sep', 
	cast([10] as int) as 'Oct', 
	cast([11] as int) as 'Nov', 
	cast([12] as int) as 'Dec'
from
(
	select min(year(signindate)) as year, min(month(signindate)) as month, cardnum
	from 
	(
		SELECT ASIs.dwccardnum as cardnum, MIN(dateStart) as signindate
		FROM dbo.Activities Acts
		JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
		JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
		WHERE dateStart >= @beginDate AND dateStart <= @enddate AND
		(
			Ls.[key] = 'Yardwork' OR
			Ls.[key] = 'SafetyLaborRights' OR
			Ls.[key] = 'RenewErgonomic' OR
			Ls.[key] = 'Moving' OR
			Ls.[key] = 'HomecareErgonomic' OR
			Ls.[key] = 'GreenClean' OR
			Ls.[key] = 'Gardening' OR
			Ls.[key] = 'Ergonomic' OR
			Ls.[key] = 'ElectricalHazards' OR
			Ls.[key] = 'ChemicalHazards' OR
			Ls.[key] = 'CaregiversClass'
		)

GROUP BY dwccardnum
	) 
	as result_set
	group by  cardnum
) as foo
PIVOT  
(  
count (cardnum)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt 

union 

select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-CityReport-ESL-' + convert(varchar(4), [year]) as id, 
    'Counts of members who accessed at least 12 hours of ESL classes' as label, 
    cast(year as int) as year, 
	cast([1] as int) as 'Jan', 
	cast([2] as int) as 'Feb', 
	cast([3] as int) as 'Mar', 
	cast([4] as int) as 'Apr',
	cast([5] as int) as 'May', 
	cast([6] as int) as 'Jun', 
	cast([7] as int) as 'Jul', 
	cast([8] as int) as 'Aug',
	cast([9] as int) as 'Sep', 
	cast([10] as int) as 'Oct', 
	cast([11] as int) as 'Nov', 
	cast([12] as int) as 'Dec'
from
(
	select year, month, cardnum
	from 
	(
			SELECT  year(dateStart) as year, month(datestart) as month, dwccardnum as cardnum,
			sum(DATEDIFF( minute, dateStart, dateEnd )) as Minutes
			from dbo.Activities Acts

			JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
			JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
			WHERE text_en LIKE '%English%'
			AND dateStart >= @beginDate AND dateend <= @EnDdate
			group by year(datestart), month(datestart), dwccardnum
	) as foo
	where foo.minutes >= 720

) as foo
PIVOT  
(  
count (cardnum)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt

union 

select
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-CityReport-UndupCount-' + convert(varchar(4), [year]) as id, 

    'A2H1-0 count of unduplicated individuals securing day labor employment this period' as label, 
    cast(year as int) as year, 
	cast(isnull([1],0) as int) as 'Jan', 
	cast(isnull([2],0) as int) as 'Feb', 
	cast(isnull([3],0) as int) as 'Mar', 
	cast(isnull([4],0) as int) as 'Apr',
	cast(isnull([5],0) as int) as 'May', 
	cast(isnull([6],0) as int) as 'Jun', 
	cast(isnull([7],0) as int) as 'Jul', 
	cast(isnull([8],0) as int) as 'Aug',
	cast(isnull([9],0) as int) as 'Sep', 
	cast(isnull([10],0) as int) as 'Oct', 
	cast(isnull([11],0) as int) as 'Nov', 
	cast(isnull([12],0) as int) as 'Dec'
from
(
	select count(cardnum) [cardnum], year(time) [year], month(time) [month]
	from 
	(
		SELECT dwccardnum AS cardnum, min(dateTimeofWork) time
		from dbo.WorkAssignments WAs
		JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
		JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
		join dbo.lookups l on l.id = wos.status
		WHERE dateTimeofWork >= @beginDate 
		and dateTimeofWork <= @EnDdate
		and l.text_EN = 'Completed'
		group by dwccardnum
	) as meh
	group by year(time), month(time)
) as foo
PIVOT  
(  
sum (cardnum)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt
"
            },
            // NewEmployersByType
            new ReportDefinition
            {
                name = "NewEmployersByType",
                commonName = "New employers by type",
                description = "A count of new employers by type (business/individual) for the given tiem range",
                category = "Employers",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-' + convert(varchar(5), business) as id,
case
  when business = 0 then 'Individual'
  when business = 1 then 'Business'
end as [Business type], 
[count] as [Count]
FROM (
SELECT
business, count(*) as [count]
FROM dbo.Employers Es
WHERE Es.datecreated >= @beginDate AND Es.datecreated <= @EnDdate
group by business
) as WW"
            },
            // DispatchSummary
            new ReportDefinition
            {
                name = "DispatchSummary",
                commonName = "Dispatch summary",
                description = "A count of distinct members dispatched and a count of total dispatches in the given time range",
                category = "Dispatches",
                sqlquery =
@"SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-total' as id,
COUNT(DISTINCT(dwccardnum)) AS [Unduplicated dispatches], 
COUNT(WAs.ID) AS [Total dispatches]
FROM dbo.WorkAssignments WAs
JOIN dbo.WorkOrders WOs on WAs.WorkOrderID = WOs.ID
JOIN dbo.Workers Ws on WAs.WorkerAssignedID = Ws.ID
join dbo.lookups l on wos.status = l.id
WHERE WOs.dateTimeOfWork >= @beginDate 
AND WOs.dateTimeOfWork <= @enddate
and l.text_en = 'Completed'"
            },
            // DispatchesByZipCodeAndCatecory
            new ReportDefinition
            {
                name = "DispatchesByZipCodeAndCatecory",
                commonName = "Dispatches by zipcode and category",
                description = "Counts of dispatches by zip code, grouped by catgory. The categories are defined in teh SQL and are based on the skill needed",
                category = "Dispatches",
                sqlquery =
@"
select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByZipCodeAndCatecory-' + isnull(zipcode, 'Total') as id,
isnull(zipcode, 'Total') as zipcode,
sum(
	isnull([cl],0)+
	isnull([yw],0)+
	isnull([gl],0)+
	isnull([gd],0)+
	isnull([mv],0)+
	isnull([pt],0)+
	isnull([hl],0)+
	isnull([dg],0)+
	isnull([ld],0)+
	isnull([dm],0)+
	isnull([ot],0)
) as Total,
sum(isnull([cl],0)) as 'Cleaning',
sum(isnull([yw],0)) as 'Yardwork',
sum(isnull([gl],0)) as 'General labor',
sum(isnull([gd],0)) as 'Gardening',
sum(isnull([mv],0)) as 'Moving',
sum(isnull([pt],0)) as 'Painting',
sum(isnull([hl],0)) as 'Hauling',
sum(isnull([dg],0)) as 'Digging',
sum(isnull([ld],0)) as 'Landscaping',
sum(isnull([dm],0)) as 'Demolition',
sum(isnull([ot],0)) as 'Other'
from
(
	select zipcode, 
	worktype,
	count(worktype) as [count]
	from
	(
		select isnull(wos.zipcode,0) as zipcode, 
		was.id as [waid], 
		case
			WHEN LOs.text_en like '%cleaning%' then 'CL'
			WHEN LOs.text_en like '%yardwork%' then 'YW'
			WHEN LOs.text_en like '%general%labor%' then 'GL'
			WHEN LOs.text_en like '%gardening%' then 'GD'
			WHEN LOs.text_en like '%moving%' then 'MV'
			WHEN LOs.text_en like '%painting%' then 'PT'
			WHEN LOs.text_en like '%hauling%' then 'HL'
			WHEN LOs.text_en like '%digging%' then 'DG'
			WHEN LOs.text_en like '%landscaping%' then 'LD'
			WHEN LOs.text_en like '%demolition%' then 'DM'
			else 'OT'
		end as worktype
		from workorders wos 
		JOIN dbo.WorkAssignments WAs ON WOs.ID = WAs.workOrderID
		JOIN dbo.Lookups LOs ON WAs.skillID = LOs.ID
		join dbo.lookups l on WOs.status = l.id
		where [dateTimeOfWork] >= @beginDate
		AND [dateTimeOfWork] <= @enddate
		and l.text_en = 'Completed'

	) as foo
	group by foo.zipcode, foo.worktype
) as goo
PIVOT  
(  
sum ([count])  
FOR worktype IN  
( [cl], [yw], [gl], [gd], [mv], [pt], [hl], [dg], [ld], [dm], [ot] )  
) AS pvt
group by rollup (pvt.zipcode)
order by total desc"
            },
            // DispatchesByZipCodeAndEmployerType
            new ReportDefinition
            {
                name = "DispatchesByZipCodeAndEmployerType",
                commonName = "Dispatches by zipcode and employer type",
                description = "Counts of dispatches by zip code, grouped by employer type, which is either individual or business",
                category = "Dispatches",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByZipCodeAndEmployerType-' + isnull(zipcode, 'Total') as id,
isnull(zipcode, 'Total') as zipcode,
sum(
	isnull([I],0)+
	isnull([B],0)
) as Total,
sum(isnull([I],0)) as 'Individual',
sum(isnull([B],0)) as 'Business'
from
(
	select zipcode, 
	employertype,
	count(employertype) as [count]
	from
	(
		select isnull(wos.zipcode,0) as zipcode, 
		was.id as [waid], 
		case
			WHEN e.business = 1  then 'B'
			else 'I'
		end as employertype
		from workorders wos 
		join employers e on wos.employerid = e.id
		JOIN dbo.WorkAssignments WAs ON WOs.ID = WAs.workOrderID
		join dbo.lookups l on WOs.status = l.id
		where [dateTimeOfWork] >= @beginDate
		AND [dateTimeOfWork] <= @enddate
		and l.text_en = 'Completed'

	) as foo
	group by foo.zipcode, foo.employertype
) as goo
PIVOT  
(  
sum ([count])  
FOR employertype IN  
( [I],[B] )  
) AS pvt
group by rollup (pvt.zipcode)
order by total desc"
            },
            // Worker details -- events
            new ReportDefinition
            {
                name = "WorkerDetailsEvents",
                commonName = "Worker Details, events (rap sheet)",
                description = "A list of events, which are complaints, recommendatiosn, sanctions, etc. for a given worker",
                category = "WorkerDetail",
                sqlquery =
@"SELECT     
l.text_en AS eventType, 
CONVERT(VARCHAR(11), dateFrom, 100) AS evDateFrom, 
CONVERT(VARCHAR(11), dateTo, 100) AS evDateTo, 
notes, 
ev.datecreated AS evDateCreated, 
ev.dateupdated AS evDateUpdated, 
ev.Createdby AS evCreatedby, 
ev.Updatedby AS evUpdatedby
FROM         Events AS ev
join workers w on (ev.PersonID = w.id)
join lookups l on (ev.eventType = l.id)
WHERE     (w.dwccardnum = @dwccardnum)",
                inputsJson = "{\"beginDate\":false,\"endDate\":false,\"memberNumber\":true}"
            },
            // Worker details -- jobs summary
            new ReportDefinition
            {
                name = "WorkerDetailsJobsSummary",
                commonName = "Worker Details, monthly summary within time range",
                description = "",
                category = "WorkerDetail",
                inputsJson = "{\"beginDate\":true,\"endDate\":true,\"memberNumber\":true}",
                sqlquery =
@"select 
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkerDetailsJobsSummary-' + convert(varchar(6), @dwccardnum) + '-' + CONVERT(VARCHAR(7), dateTimeofWork, 102)as id,
CONVERT(VARCHAR(7), dateTimeofWork, 102) as Month,
count(*) as [Job Count],
sum(hours) as [Hours worked],
sum(income) as [Monthly income]
from
(
	SELECT      
	wa.hours, 
	CONVERT(money, wa.hours * wa.hourlyWage) AS income, 
	wo.dateTimeofWork AS dateTimeofWork
	FROM         
	WorkAssignments AS wa INNER JOIN
	WorkOrders AS wo ON wa.workOrderID = wo.ID
	inner join workers as w on (w.id = wa.workerassignedID)
	WHERE w.dwccardnum = @dwccardnum
		and WO.dateTimeofWork > @beginDate
		and WO.dateTimeofWork < @endDate
) as foo
group by
 CONVERT(VARCHAR(7), dateTimeofWork, 102)"
            },
            // Worker details -- jobs itemized
            new ReportDefinition
            {
                name = "WorkerDetailsJobsItemized",
                commonName = "Worker details, itemized jobs within time range",
                description = "",
                category = "WorkerDetail",
                inputsJson = "{\"beginDate\":true,\"endDate\":true,\"memberNumber\":true}",
                sqlquery =
@"SELECT   
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkerDetailsJobsItemized-' + convert(varchar(6), @dwccardnum) + '-' + CONVERT(VARCHAR(7), wa.id)as id, 
l.text_en AS skill, 
wa.hours, 
CONVERT(money, wa.hourlyWage) AS hourlyWage, 
CONVERT(money, wa.hours * wa.hourlyWage) AS income, 
RIGHT('00000' + CAST(wo.paperOrderNum AS varchar(5)), 5) + '-' + RIGHT('00' + CAST(wa.pseudoID AS varchar(2)), 2) AS compositeOrderNum, 
CONVERT(VARCHAR(16), wo.dateTimeofWork, 120) AS dateTimeofWork, 
wo.contactName
FROM         
WorkAssignments AS wa 
INNER JOIN WorkOrders AS wo ON wa.workOrderID = wo.ID
inner join workers as w on (w.id = wa.workerassignedID)
inner join Lookups l on (l.id = wa.skillID)
WHERE     (w.dwccardnum = @dwccardnum)
		and WO.dateTimeofWork > @beginDate
		and WO.dateTimeofWork < @endDate"
            },
            // Worker details -- signin summary
            new ReportDefinition
            {
                name = "WorkerDetailsSigninSummary",
                commonName = "Worker details, signin summary",
                description = "",
                category = "WorkerDetail",
                inputsJson = "{\"beginDate\":true,\"endDate\":true,\"memberNumber\":true}",
                sqlquery =
@"select
	convert(varchar(7), signindate, 102) as YearMonth,
	case when (sum(cast([1] as int))) > 0 then 'X' else '' end as '1',
	case when (sum(cast([2] as int))) > 0 then 'X' else '' end as '2',
	case when (sum(cast([3] as int))) > 0 then 'X' else '' end as '3',
	case when (sum(cast([4] as int))) > 0 then 'X' else '' end as '4',
	case when (sum(cast([5] as int))) > 0 then 'X' else '' end as '5',
	case when (sum(cast([6] as int))) > 0 then 'X' else '' end as '6',
	case when (sum(cast([7] as int))) > 0 then 'X' else '' end as '7',
	case when (sum(cast([8] as int))) > 0 then 'X' else '' end as '8',
	case when (sum(cast([9] as int))) > 0 then 'X' else '' end as '9',
	case when (sum(cast([10] as int))) > 0 then 'X' else '' end as '10',
	case when (sum(cast([11] as int))) > 0 then 'X' else '' end as '11',
	case when (sum(cast([12] as int))) > 0 then 'X' else '' end as '12',
	case when (sum(cast([13] as int))) > 0 then 'X' else '' end as '13',
	case when (sum(cast([14] as int))) > 0 then 'X' else '' end as '14',
	case when (sum(cast([15] as int))) > 0 then 'X' else '' end as '15',
	case when (sum(cast([16] as int))) > 0 then 'X' else '' end as '16',
	case when (sum(cast([17] as int))) > 0 then 'X' else '' end as '17',
	case when (sum(cast([18] as int))) > 0 then 'X' else '' end as '18',
	case when (sum(cast([19] as int))) > 0 then 'X' else '' end as '19',
	case when (sum(cast([20] as int))) > 0 then 'X' else '' end as '20',
	case when (sum(cast([21] as int))) > 0 then 'X' else '' end as '21',
	case when (sum(cast([22] as int))) > 0 then 'X' else '' end as '22',
	case when (sum(cast([23] as int))) > 0 then 'X' else '' end as '23',
	case when (sum(cast([24] as int))) > 0 then 'X' else '' end as '24',
	case when (sum(cast([25] as int))) > 0 then 'X' else '' end as '25',
	case when (sum(cast([26] as int))) > 0 then 'X' else '' end as '26',
	case when (sum(cast([27] as int))) > 0 then 'X' else '' end as '27',
	case when (sum(cast([28] as int))) > 0 then 'X' else '' end as '28',
	case when (sum(cast([29] as int))) > 0 then 'X' else '' end as '29',
	case when (sum(cast([30] as int))) > 0 then 'X' else '' end as '30',
	case when (sum(cast([31] as int))) > 0 then 'X' else '' end as '31'
from
(
	SELECT
	  wsi.WorkerID as workerID
	  ,wsi.dateforsignin as signindate
	  ,day(wsi.dateforsignin) as [day]
	FROM
	  WorkerSignins wsi
	where dwccardnum = @dwccardnum
	and wsi.dateforsignin >= @beginDate
	and wsi.dateforsignin <= @endDate
) as foo
pivot
(
count (workerID)
for [day] in 

( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31] )
) as pvt
group by convert(varchar(7), signindate, 102)"
            },
            // Activity ttendance by activity
            new ReportDefinition
            {
                name = "ActivityAttendance",
                commonName = "Activity attendance by activity",
                description = "",
                category = "Activities",
                sqlquery = @"SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-ActivityAttendanceByType-' + convert(varchar(5), min(a.name)) as id,
l.text_en [Activity], 
count(*) [Count],
count(distinct(asi.dwccardnum)) [Unique workers]
from
activities a
join activitysignins asi on a.id = asi.activityid
join lookups l on l.id = a.name
where a.datestart > @beginDate
and a.datestart < @enddate
group by l.text_en

union
SELECT
convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-ActivityAttendanceByType-TOTAL' as id,
'Total' as [Activity], 
count(*) [Count],
count(distinct(asi.dwccardnum)) [Unique workers]
from
activities a
join activitysignins asi on a.id = asi.activityid
join lookups l on l.id = a.name
where a.datestart > @beginDate
and a.datestart < @enddate
order by count desc"
            },
            // 
            new ReportDefinition
            {
                name = "UniqueDispatchedWorkers",
                commonName = "Unique dispatched workers",
                description = "A list of unique workers within the given time period. Job count included.",
                category = "Dispatches",
                sqlquery = @"SELECT distinct(dwccardnum) AS [Member number], min(fullname) as [Name], count(*) as [Job count]
	from dbo.WorkAssignments WAs
	JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
	JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
	join dbo.lookups l on l.id = wos.status
	join dbo.persons p on p.id = ws.id
	WHERE dateTimeofWork >= @beginDate 
	and dateTimeofWork <= @EnDdate
	and l.text_EN = 'Completed'
	group by dwccardnum, fullname
	order by [Job count] desc"
            },
            new ReportDefinition
            {
                name = "MemberAttendanceMetrics",
                commonName = "Member Attendance metrics",
                description = "A list of unique members within a given time period, with counts of dispatches, activities, and ESL classes for the period.",
                category = "Attendance",
                sqlquery =
@"with jobs (dwccardnum, Jobcount)
as
(
	SELECT dwccardnum, count(*) as [Jobcount]
	from dbo.WorkAssignments WAs
	JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
	JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
	join dbo.lookups l on l.id = wos.status
	WHERE dateTimeofWork >= @begindate 
	and dateTimeofWork <= @EnDdate
	and l.text_EN = 'Completed'
	group by dwccardnum
),
act (dwccardnum, actcount)
as
(
	select dwccardnum, count(*) as [actcount]
	from activitysignins asi
	where dateforsignin >= @begindate
	and dateforsignin <= @enddate
	group by dwccardnum
),
esl (dwccardnum, eslcount)
as
(
	select asi1.dwccardnum, count(*) as [eslcount]
	from activitysignins asi1
	join activities aa on aa.ID = asi1.activityid

	where aa.nameen in ('English Class 1', 'English Class 2', 'Somos Vecinos')
	and asi1.dateforsignin >= @begindate
	and asi1.dateforsignin <= @enddate
	group by asi1.dwccardnum
),
cardnums (dwccardnum)
as
(
	 select dwccardnum from jobs 
	  union 
	  select dwccardnum from act
	  union
	  select dwccardnum from esl
	
)
select distinct(cn.dwccardnum) [Member number]
, p.fullname [Member name]
, cast(isnull([jobcount],0) as int) as  [Dispatches]
, cast(isnull([actcount],0) as int) as [Activities]
, cast(isnull([eslcount],0) as int) as [ESL]
from cardnums cn 
join workers w on cn.dwccardnum = w.dwccardnum
join persons p on w.id = p.id
left join jobs on cn.dwccardnum = jobs.dwccardnum
left join act on jobs.dwccardnum = act.dwccardnum
left join esl on esl.dwccardnum = jobs.dwccardnum

where jobcount is not null or actcount is not null or eslcount is not null
"
            }           
            #endregion  
        };
        public static void Initialize(MacheteContext context)
        {
            var inputsStub = new
            {
                // TODO make model class
                beginDate = true,
                beginDateDefault = DateTime.Parse("1/1/2016"),
                endDate = true,
                endDateDefault = DateTime.Parse("1/1/2017"),
                memberNumber = false
            };
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
                    u.columnsJson = SqlServerUtils.getUIColumnsJson(context, u.sqlquery);
                    if (u.inputsJson == null)
                    {
                        u.inputsJson = JsonConvert.SerializeObject(inputsStub);
                    }
                    context.ReportDefinitions.Add(u);
                }
            });
            context.Commit();
        }
    }

}
