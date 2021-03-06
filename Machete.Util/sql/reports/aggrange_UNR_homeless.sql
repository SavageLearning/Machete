declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLivingSituation-' + min(homeless) as id,
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
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.homeless
) as WW
group by homeless
union
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByLivingSituation-TOTAL'  as id,
'Total' as [Homeless?], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   group by W.ID
) as WWW