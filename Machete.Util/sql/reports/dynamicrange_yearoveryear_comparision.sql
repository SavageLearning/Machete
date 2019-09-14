declare @startDate DateTime = '1/1/2011';
declare @endDate DateTime = '1/1/2017';

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByMonthYearToYear-' + convert(varchar(4), [year])  as id,
year, 
	[1] as 'Jan', [2] as 'Feb', [3] as 'Mar', [4] as 'Apr',
	[5] as 'May', [6] as 'Jun', [7] as 'Jul', [8] as 'Aug',
	[9] as 'Sep', [10] as 'Oct', [11] as 'Nov', [12] as 'Dec'
from
(
	SELECT COUNT(WAs.[ID]) as jobDisp

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
sum (jobdisp)  
FOR month IN  
( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  
) AS pvt
 
ORDER BY pvt.year; 
