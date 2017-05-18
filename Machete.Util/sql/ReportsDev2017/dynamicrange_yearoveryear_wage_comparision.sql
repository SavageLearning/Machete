declare @startDate DateTime = '1/1/2011';
declare @endDate DateTime = '1/1/2017';

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-AverageWageByMonthYearToYear-' + convert(varchar(4), [year])  as id,
year, 
	cast([1] as float) as 'Jan', 
	cast([2] as float) as 'Feb', 
	cast([3] as float) as 'Mar', 
	cast([4] as float) as 'Apr',
	cast([5] as float) as 'May', 
	cast([6] as float) as 'Jun', 
	cast([7] as float) as 'Jul', 
    cast([8] as float) as 'Aug',
	cast([9] as float) as 'Sep', 
	cast([10] as float) as 'Oct',
	cast([11] as float) as 'Nov', 
	cast([12] as float) as 'Dec'
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
