declare @start dateTime = '1/1/2016'
declare @end dateTime = '1/1/2017'

SELECT Ls.text_EN, count(Ls.ID)
--CASE WHEN Ls.ID = 38 THEN 1 ELSE 0 END AS Gender
from dbo.Persons Ps
JOIN dbo.Workers Ws on Ps.ID = Ws.ID
JOIN dbo.Lookups Ls on Ps.gender = Ls.ID
WHERE dateofMembership <= @end 
AND memberexpirationdate >= @start
group by Ls.text_EN

SELECT
COUNT(DISTINCT(CASE WHEN business = 0 THEN Es.ID END)) AS Inds,
COUNT(DISTINCT(CASE WHEN business = 1 THEN Es.ID END)) AS Bizs
FROM dbo.Employers Es
WHERE Es.datecreated >= @start AND Es.datecreated <= @end

SELECT COUNT(DISTINCT(dwccardnum)) AS UndupDispatch, COUNT(WAs.ID) AS TotalDispatch
FROM dbo.WorkAssignments WAs
JOIN dbo.WorkOrders WOs on WAs.WorkOrderID = WOs.ID
JOIN dbo.Workers Ws on WAs.WorkerAssignedID = Ws.ID
WHERE WOs.dateTimeOfWork >= @start AND WOs.dateTimeOfWork <= @end