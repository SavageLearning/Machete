declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByZipCodeAndCatecory-' + isnull(zipcode, 'Total') as id,
isnull(zipcode, 'Total') as zipcode,
sum(
	isnull([cl],0)+
	isnull([yw],0)+
	isnull([gl],0)+
	isnull([gd],0)+
	isnull([mv],0)+
	isnull([pt],0)+
	isnull([hl],0)+
	isnull([dg],0)+
	isnull([ld],0)+
	isnull([dm],0)+
	isnull([ot],0)
) as Total,
sum(isnull([cl],0)) as 'Cleaning',
sum(isnull([yw],0)) as 'Yardwork',
sum(isnull([gl],0)) as 'General labor',
sum(isnull([gd],0)) as 'Gardening',
sum(isnull([mv],0)) as 'Moving',
sum(isnull([pt],0)) as 'Painting',
sum(isnull([hl],0)) as 'Hauling',
sum(isnull([dg],0)) as 'Digging',
sum(isnull([ld],0)) as 'Landscaping',
sum(isnull([dm],0)) as 'Demolition',
sum(isnull([ot],0)) as 'Other'
from
(
	select zipcode, 
	worktype,
	count(worktype) as [count]
	from
	(
		select isnull(wos.zipcode,0) as zipcode, 
		was.id as [waid], 
		case
			WHEN LOs.text_en like '%cleaning%' then 'CL'
			WHEN LOs.text_en like '%yardwork%' then 'YW'
			WHEN LOs.text_en like '%general%labor%' then 'GL'
			WHEN LOs.text_en like '%gardening%' then 'GD'
			WHEN LOs.text_en like '%moving%' then 'MV'
			WHEN LOs.text_en like '%painting%' then 'PT'
			WHEN LOs.text_en like '%hauling%' then 'HL'
			WHEN LOs.text_en like '%digging%' then 'DG'
			WHEN LOs.text_en like '%landscaping%' then 'LD'
			WHEN LOs.text_en like '%demolition%' then 'DM'
			else 'OT'
		end as worktype
		from workorders wos 
		JOIN dbo.WorkAssignments WAs ON WOs.ID = WAs.workOrderID
		JOIN dbo.Lookups LOs ON WAs.skillID = LOs.ID
		join dbo.lookups l on WOs.status = l.id
		where [dateTimeOfWork] >= @startdate
		AND [dateTimeOfWork] <= @enddate
		and l.text_en = 'Completed'

	) as foo
	group by foo.zipcode, foo.worktype
) as goo
PIVOT  
(  
sum ([count])  
FOR worktype IN  
( [cl], [yw], [gl], [gd], [mv], [pt], [hl], [dg], [ld], [dm], [ot] )  
) AS pvt
group by rollup (pvt.zipcode)
order by total desc