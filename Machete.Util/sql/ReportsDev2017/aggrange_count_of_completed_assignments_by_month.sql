declare @start dateTime = '1/1/2017'
declare @end dateTime = '1/1/2018'
select month(wo.datetimeofwork) month, count(*)
from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @start 
AND datetimeofwork < @end
and l.text_en = 'Completed'
and wa.workerassignedid is not null
group by month(wo.datetimeofwork)