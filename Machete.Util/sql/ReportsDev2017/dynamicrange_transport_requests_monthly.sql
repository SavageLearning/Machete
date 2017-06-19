declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-TransportationFeeMonthly-' + CONVERT(VARCHAR(7), workStartTime, 102)as id,
CONVERT(VARCHAR(7), workStartTime, 102) as Month,
count (wonum) as [Order count],
sum(wkrcount) as [Worker count],
sum(transfee) as [Transport fee],
sum(exfee) as [Extra fee]
from
(
	select
	   WOs.dateTimeofWork AS workStartTime,
	   WOs.paperOrderNum as wonum,
	   text_EN as typeOfWork,
	   name as employerName,
	   COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as wkrCount,
	   transportFee as transFee,
	   transportFeeExtra as exFee
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
) as foo
group by CONVERT(VARCHAR(7), workStartTime, 102)