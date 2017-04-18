declare @start dateTime = '1/1/2016'
declare @end dateTime = '1/1/2017'

select distinct(dwccardnum) 
from 
(
	select distinct(w.dwccardnum)
	from workassignments wa
	join workorders wo on wo.id = wa.workorderid
	join lookups     l on wo.status = l.id
	join workers     w on w.id = wa.workerassignedid
	where datetimeofwork >= @start 
	AND datetimeofwork < @end
	and l.text_en = 'Completed'

	union 

	select distinct(dwccardnum)
	from activitysignins asi
	where dateforsignin >= @start
	and dateforsignin < @end
) 
as foo
order by  dwccardnum