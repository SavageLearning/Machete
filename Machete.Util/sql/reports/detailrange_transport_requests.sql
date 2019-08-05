declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-TransportationFeeDetails-' + convert(varchar(6), min(WOs.paperOrderNum)) as id,
   WOs.dateTimeofWork AS [Work start time],
   WOs.paperOrderNum as [WO number],
   text_EN as [Transport type],
   name as [Employer name],
   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as [Wkr count],
   transportFee as [Transport fee],
   transportFeeExtra as [Extra fee]
from dbo.Lookups Ls
join dbo.WorkOrders WOs on Ls.ID = WOs.transportMethodID
join dbo.Employers Es on WOs.EmployerID = Es.ID
join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
where category like 'transportmethod' 
	and (transportFee + transportFeeExtra) > 0
    and WOs.dateTimeofWork > @startDate
	and WOs.dateTimeofWork < @endDate
	and WAs.workerAssignedID IS NOT NULL
group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra