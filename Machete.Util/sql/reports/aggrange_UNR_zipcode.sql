declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByZipcode-' + min(zipcode) as id,
zipcode as Zipcode, 
count(*) as [Count]
FROM (
  select W.ID, isnull(w.zipcode, 'NULL') as zipcode
  from Persons W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.zipcode
) as WW
group by zipcode
union
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByZipcode-TOTAL'  as id,
'Total' as [Zipcode], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   group by W.ID
) as WWW