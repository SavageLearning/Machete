declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'

SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByJob-' + convert(varchar(5), min(wa.skillid)) as id,
lskill.text_en  AS [Job type (skill)],
count(lskill.text_en) [Count]
FROM [dbo].WorkAssignments as WA 
join [dbo].lookups as lskill on (wa.skillid = lskill.id)
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@startDate)
and lstatus.text_en = 'Completed'
group by lskill.text_en
union 
SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByJob-TOTAL' as id,
'Total' AS [Job type (skill)],
count(*) [Count]
FROM [dbo].WorkAssignments as WA 
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@startDate)
and lstatus.text_en = 'Completed'
