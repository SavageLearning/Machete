declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'
--select month(wo.datetimeofwork) month, count(*)
--from workassignments wa
--join workorders wo on wo.id = wa.workorderid
--join lookups l on wo.status = l.id
--where  datetimeofwork >= @start 
--AND datetimeofwork < @end
--and l.text_en = 'Completed'
--and wa.workerassignedid is not null
--group by month(wo.datetimeofwork)

SELECT
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
group by month(wo.datetimeofwork)