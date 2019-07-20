declare @begindate datetime = '2019-05-01T00:00:00'

declare @endate datetime = '2019-05-31T23:59:59'
;with cte as (   
	select 
		convert(varchar, wo.datetimeofwork, 23) as workdate, 
		sum(wa.hours) as [hours] , 
		sum(wa.hours * wa.hourlyWage) as [TotalIncome], 
		AVG((wa.hours*wa.hourlywage)/wa.hours) as [average]

	from WorkOrders wo
	join WorkAssignments wa
	on wa.workOrderID = wo.id

	group by convert(varchar, wo.datetimeofwork, 23)
)

select 
	convert(varchar, dateforsignin, 23) as dateforsignin
	, (select count(*) from dbo.WorkerSignins where dateforsignin > @begindate 
	and dateforsignin < @endate) as [active]
	, count(w.dwccardnum) as [signins]
	, avg(cte.hours) as [hours]
	, avg(cte.TotalIncome) as [totalIncome]
	, avg(cte.average) as [average]

from dbo.workers w 
join dbo.WorkerSignins wasi
on wasi.WorkerID = w.id 
join cte 
on cte.workdate = convert(varchar, dateforsignin, 23)

where dateforsignin > @begindate 
and dateforsignin < @endate

group by convert(varchar, dateforsignin, 23)