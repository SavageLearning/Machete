declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'

SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonth-' + convert(varchar(5), month(min(wo.datetimeofwork))) as id,
convert(varchar(7), min(wo.datetimeofwork), 126)  AS [Month],
count(*) [Count]
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @startDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'

group by month(wo.datetimeofwork)

union 

SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonth-TOTAL' as id,
'Total'  AS [Month],
count(*) [Count]
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @startDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'

