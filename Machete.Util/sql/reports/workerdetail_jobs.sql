declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';
declare @dwccardnum integer = 30533;


SELECT   
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkerDetailsJobsItemized-' + convert(varchar(6), @dwccardnum) + '-' + CONVERT(VARCHAR(7), wa.id)as id, 
l.text_en AS skill, 
wa.hours, 
CONVERT(money, wa.hourlyWage) AS hourlyWage, 
cast(CONVERT(money, wa.hours * wa.hourlyWage) as float) AS income, 
RIGHT('00000' + CAST(wo.paperOrderNum AS varchar(5)), 5) + '-' + RIGHT('00' + CAST(wa.pseudoID AS varchar(2)), 2) AS compositeOrderNum, 
CONVERT(VARCHAR(16), wo.dateTimeofWork, 120) AS dateTimeofWork, 
wo.contactName
FROM         
WorkAssignments AS wa 
INNER JOIN WorkOrders AS wo ON wa.workOrderID = wo.ID
inner join workers as w on (w.id = wa.workerassignedID)
inner join Lookups l on (l.id = wa.skillID)
WHERE     (w.dwccardnum = @dwccardnum)
		and WO.dateTimeofWork > @startDate
		and WO.dateTimeofWork < @endDate