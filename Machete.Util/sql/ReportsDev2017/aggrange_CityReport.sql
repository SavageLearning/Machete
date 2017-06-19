declare @start dateTime = '1/1/2016'
declare @end dateTime = '1/1/2017'
-- newly enrolled
SELECT MIN(dateforsignin) AS Date, dwccardnum
FROM dbo.WorkerSignins
WHERE dateforsignin >= @start AND
dateforsignin < @end
GROUP BY dwccardnum
UNION
SELECT MIN(dateforsignin) AS Date, dwccardnum
FROM dbo.ActivitySignins
WHERE dateforsignin >=@start AND
dateforsignin < @end
GROUP BY dwccardnum

--financial literacy
SELECT ASIs.dwccardnum, MIN(dateStart) as Date
FROM dbo.Activities Acts
JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
WHERE Ls.ID = 179 AND dateStart >= @start AND dateStart <= @end
GROUP BY ASIs.dwccardnum

--skills training or workshops
SELECT ASIs.dwccardnum, MIN(dateStart)
FROM dbo.Activities Acts
JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
WHERE dateStart >= @start AND dateStart <= @end AND
(Ls.ID = 182 OR Ls.ID = 181 OR Ls.ID = 180
OR Ls.ID = 179 OR Ls.ID = 178 OR Ls.ID = 134
OR Ls.ID = 168 OR Ls.ID = 156 OR Ls.ID = 152
OR Ls.ID = 145 OR Ls.ID = 135 OR Ls.ID = 104)
GROUP BY dwccardnum

-- esl classes in the last 12 months
SELECT COUNT(Acts.ID) as ActID, MIN(dateStart) as Date, dwccardnum as Member, 'English Class' as ClassName,
SUM ( DATEDIFF( minute, dateStart, dateEnd )) as Minutes,
CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 720 THEN 1
ELSE 0
END AS [12]
from dbo.Activities Acts

JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
WHERE text_en LIKE 'English%'
AND dateStart >= @start AND dateend <= @end

GROUP BY dwccardnum

--A2H1-0 - number of unduplicated individuals who secured day labor 
SELECT dwccardnum AS cardnum, MIN(dateTimeofWork) AS time
from dbo.WorkAssignments WAs
JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
WHERE dateTimeofWork >= @start and dateTimeofWork <= @end

GROUP BY dwccardnum