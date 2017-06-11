declare @startDate DateTime = '4/16/2017 11:41:11 AM';
declare @endDate DateTime = '5/16/2017 11:41:11 AM';

with demos_esl(id, [Member ID], [minutes], [12 hr completion], [24+ hr completion])
as
(
	select 
		convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(10), min(member)) as id,	
		cast(member as varchar) as [Member ID], 
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
		WHERE text_en LIKE 'English%'
		AND dateStart >= @startDate AND dateend <= @enddate

		GROUP BY dwccardnum
	) as foo 
	group by member
)
select * from demos_esl
union 
select 
	convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-TOTAL' as id,	
	'Totals: ' + convert(varchar(5), count(*)) as [Member ID],
	sum([minutes]) as [minutes],
	sum([12 hr completion]),
	sum([24+ hr completion])
from demos_esl