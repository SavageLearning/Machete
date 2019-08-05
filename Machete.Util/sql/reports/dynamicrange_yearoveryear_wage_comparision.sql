declare @startDate DateTime = '5/1/2016';
declare @endDate DateTime = '5/1/2017';

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-AverageWageByMonthYearToYear-' + convert(varchar(4), [year])  as id,
year, 
	cast(isnull([1],0) as float) as 'Jan', 
	cast(isnull([2],0) as float) as 'Feb', 
	cast(isnull([3],0) as float) as 'Mar', 
	cast(isnull([4],0) as float) as 'Apr',
	cast(isnull([5],0) as float) as 'May', 
	cast(isnull([6],0) as float) as 'Jun', 
	cast(isnull([7],0) as float) as 'Jul', 
    cast(isnull([8],0) as float) as 'Aug',
	cast(isnull([9],0) as float) as 'Sep', 
	cast(isnull([10],0) as float) as 'Oct',
	cast(isnull([11],0) as float) as 'Nov', 
	cast(isnull([12],0) as float) as 'Dec'
from
(
	SELECT CONVERT(DECIMAL(10,2),AVG([hourlyWage])) AS avgWage
	,year([dateTimeOfWork]) AS year
	,month([dateTimeOfWork]) as month
	FROM [dbo].[WorkAssignments] WAs
	JOIN [dbo].[WorkOrders] WOs ON WAs.workOrderID = WOs.ID
	join [dbo].[Lookups] l on wos.status = l.id
	WHERE [workerAssignedID] IS NOT NULL
	AND [dateTimeOfWork] >= @startdate
	AND [dateTimeOfWork] <= @enddate
	and l.text_en = 'Completed'
	GROUP BY year([dateTimeOfWork]), month([dateTimeOfWork])
) as foo
PIVOT  
(  
sum (avgWage)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt 
