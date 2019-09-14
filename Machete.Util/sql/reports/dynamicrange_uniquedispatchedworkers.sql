	declare @startDate DateTime = '5/1/2016'
declare @endDate DateTime = '5/1/2017'

	
	SELECT distinct(dwccardnum) AS [Member number], min(fullname) as [Name], count(*) as [Job count]
	from dbo.WorkAssignments WAs
	JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
	JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
	join dbo.lookups l on l.id = wos.status
	join dbo.persons p on p.id = ws.id
	WHERE dateTimeofWork >= @startdate 
	and dateTimeofWork <= @EnDdate
	and l.text_EN = 'Completed'
	group by dwccardnum, fullname
	order by [Job count] desc
