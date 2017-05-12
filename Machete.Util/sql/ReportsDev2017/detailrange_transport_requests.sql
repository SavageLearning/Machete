declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select
WOs.dateTimeofWork AS ttime,
WOs.paperOrderNum as wonum,
text_EN as type,
name as empName,
COUNT(COALESCE(firstname1,'') + ' ' + COALESCE(firstname2,'') + ' ' + COALESCE(lastname1,'') + ' ' + COALESCE(lastname2,'')) as wkName,
transportFee as transFee,
transportFeeExtra as exFee
from dbo.Lookups Ls
join dbo.WorkOrders WOs on Ls.ID = WOs.transportMethodID
join dbo.Employers Es on WOs.EmployerID = Es.ID
join dbo.WorkAssignments WAs on WOs.ID = WAs.workOrderID
join dbo.Persons Ps on WAs.workerAssignedID = Ps.ID
where category like 'transportmethod'
and (transportFee + transportFeeExtra) > 0
and WOs.dateTimeofWork > @startdate
and WOs.dateTimeofWork < @enddate
and WAs.workerAssignedID IS NOT NULL
group by WOs.dateTimeofWork, WOs.paperOrderNum, text_EN, name, transportFee, transportFeeExtra