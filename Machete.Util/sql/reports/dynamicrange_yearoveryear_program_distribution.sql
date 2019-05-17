declare @startDate DateTime = '1/1/2011';
declare @endDate DateTime = '1/1/2017';
select
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByProgramYearToYear-' + year_program as id, 
year_program, 
	isnull([1],0) as 'Jan', 
	isnull([2],0) as 'Feb', 
	isnull([3],0) as 'Mar', 
	isnull([4],0) as 'Apr',
	isnull([5],0) as 'May', 
	isnull([6],0) as 'Jun', 
	isnull([7],0) as 'Jul', 
	isnull([8],0) as 'Aug',
	isnull([9],0) as 'Sep', 
	isnull([10],0) as 'Oct', 
	isnull([11],0) as 'Nov', 
	isnull([12],0) as 'Dec'
from
(
	SELECT
	count(wa.workerAssignedID) JobCount
	,convert(varchar(4), year(wo.dateTimeofWork)) + '-' + convert(varchar(3), isnull(l.[key], 'NUL'))  AS year_program
	,month(wo.dateTimeofWork) as month
	FROM
	WorkAssignments wa
	INNER JOIN Workers w ON wa.workerAssignedID = w.ID
	INNER JOIN WorkOrders wo ON wa.workOrderID = wo.ID
	inner join lookups l on w.typeOfWorkID = l.ID
	inner join lookups ll on wo.status = ll.id
	where [dateTimeOfWork] >= @startdate
	AND [dateTimeOfWork] <= @enddate
	and ll.text_en = 'Completed'
	group by l.Text_EN, l.[key], year(wo.dateTimeofWork), month(wo.dateTimeofWork)
) as foo
PIVOT  
(  
sum (JobCOunt)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt
