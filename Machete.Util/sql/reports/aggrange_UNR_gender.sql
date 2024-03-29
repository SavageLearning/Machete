declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-' + convert(varchar(5), min(WW.gender)) as id,
L.text_EN as [Gender], 
count(*) as [Count]
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
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-NULL' as id,
'NULL' as [Gender], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from persons W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   and W.gender is null
   group by W.ID
) as WWW

union
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByGender-TOTAL'  as id,
'Total' as [Gender], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   group by W.ID
) as WWW