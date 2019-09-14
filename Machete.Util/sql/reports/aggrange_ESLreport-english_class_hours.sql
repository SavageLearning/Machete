declare @start dateTime = '1/1/2016'
declare @end dateTime = '1/1/2017'

select 
    convert(varchar(8), @beginDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-' + convert(varchar(10), min(member)) as id,	
	member as [Member ID], 
	sum(minutes) as [Total minutes],
	min([12hrTier]) as [12 hr completion],
	min([24hrTier]) as [24+ hr completion]
from
(
	SELECT COUNT(Acts.ID) as ActID,
	dwccardnum as Member,
	'English Class' as ClassName,
	SUM ( DATEDIFF( minute, dateStart, dateEnd )) as Minutes,
	CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 1440 THEN 1
	ELSE 0
	END AS [24hrTier],
	CASE WHEN SUM ( DATEDIFF ( minute, dateStart, dateEnd )) >= 720 AND SUM ( DATEDIFF ( minute, dateStart, dateEnd )) <= 1439 THEN 1
	ELSE 0
	END AS [12hrTier]
	from dbo.Activities Acts

	JOIN dbo.Lookups Ls ON Acts.name = Ls.ID
	JOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID
	WHERE 
	(
	  Ls.[key] = 'SomosVecinos' OR
	  Ls.[key] = 'EnglishClass1' OR
	  Ls.[key] = 'EnglishClass2' 
	)
	AND dateStart >= @beginDate AND dateend <= @enddate

	GROUP BY dwccardnum
) as foo 
group by member