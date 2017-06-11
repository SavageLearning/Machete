declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';
declare @dwccardnum integer = 30533;


select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkerDetailsJobsSummary-' + convert(varchar(6), @dwccardnum) + '-' + CONVERT(VARCHAR(7), dateTimeofWork, 102)as id,
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
		and WO.dateTimeofWork > @startDate
		and WO.dateTimeofWork < @endDate
) as foo
group by
 CONVERT(VARCHAR(7), dateTimeofWork, 102)

truncate table reportdefinitions