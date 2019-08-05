declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-DispatchesByZipCodeAndEmployerType-' + isnull(zipcode, 'Total') as id,
isnull(zipcode, 'Total') as zipcode,
sum(
	isnull([I],0)+
	isnull([B],0)
) as Total,
sum(isnull([I],0)) as 'Individual',
sum(isnull([B],0)) as 'Business'
from
(
	select zipcode, 
	employertype,
	count(employertype) as [count]
	from
	(
		select isnull(wos.zipcode,0) as zipcode, 
		was.id as [waid], 
		case
			WHEN e.business = 1  then 'B'
			else 'I'
		end as employertype
		from workorders wos 
		join employers e on wos.employerid = e.id
		JOIN dbo.WorkAssignments WAs ON WOs.ID = WAs.workOrderID
		join dbo.lookups l on WOs.status = l.id
		where [dateTimeOfWork] >= @startdate
		AND [dateTimeOfWork] <= @enddate
		and l.text_en = 'Completed'

	) as foo
	group by foo.zipcode, foo.employertype
) as goo
PIVOT  
(  
sum ([count])  
FOR employertype IN  
( [I],[B] )  
) AS pvt
group by rollup (pvt.zipcode)
order by total desc