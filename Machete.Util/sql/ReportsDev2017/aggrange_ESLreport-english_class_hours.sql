declare @start dateTime = '1/1/2016'
declare @end dateTime = '1/1/2017'

SELECT COUNT(Acts.ID) as ActCount,
dwccardnum as Member,
'English Class' as ClassName,
SUM ( DATEDIFF( minute, dateStart, dateEnd )) as ClassMinutes,
CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 1440 THEN 1
ELSE 0
END AS [24requirement],
CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 720 AND SUM ( DATEDIFF ( minute, dateStart, dateEnd )) <= 1439 THEN 1
ELSE 0
END AS [12requirement]
from dbo.Activities Acts

JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
WHERE text_en LIKE 'English%'
AND dateStart >= @start AND dateend <= @end

GROUP BY dwccardnum