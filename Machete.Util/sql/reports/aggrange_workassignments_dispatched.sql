declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

SELECT
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-total' as id,
COUNT(DISTINCT(dwccardnum)) AS [Unduplicated dispatches], 
COUNT(WAs.ID) AS [Total dispatches]
FROM dbo.WorkAssignments WAs
JOIN dbo.WorkOrders WOs on WAs.WorkOrderID = WOs.ID
JOIN dbo.Workers Ws on WAs.WorkerAssignedID = Ws.ID
join dbo.lookups l on wos.status = l.id
WHERE WOs.dateTimeOfWork >= @startdate 
AND WOs.dateTimeOfWork <= @enddate
and l.text_en = 'Completed'
