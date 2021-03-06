declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByArrivalStatus-' + min(immigrantrefugee) as id,
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
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.immigrantrefugee
) as WW
group by immigrantrefugee
union 
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByArrivalStatus-TOTAL'  as id,
'Total' as [Immigrant / Refugee?], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   group by W.ID
) as WWW