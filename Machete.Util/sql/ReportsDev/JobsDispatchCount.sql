select f.worktype, f.counted, (100.0 * f.counted)/ (sum(f.counted) over ()) as percentage
from
(SELECT 
	wa.skillEN  AS workType,
	count(wa.skillEN) counted
FROM [dbo].WorkAssignments as WA 
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID) 
WHERE wo.dateTimeOfWork < ('1/1/2014') 
and wo.dateTimeOfWork > ('1/1/2013')
and wo.statusEN = 'Completed'
group by wa.skillEN) as f
group by f.worktype, f.counted