declare @startDate DateTime = '5/1/2016';
declare @endDate DateTime = '5/1/2017';



with jobs (dwccardnum, Jobcount)
as
(
	SELECT dwccardnum, count(*) as [Jobcount]
	from dbo.WorkAssignments WAs
	JOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID
	JOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID
	join dbo.lookups l on l.id = wos.status
	WHERE dateTimeofWork >= @startdate 
	and dateTimeofWork <= @EnDdate
	and l.text_EN = 'Completed'
	group by dwccardnum
),
act (dwccardnum, actcount)
as
(
	select dwccardnum, count(*) as [actcount]
	from activitysignins asi
	where dateforsignin >= @startdate
	and dateforsignin <= @enddate
	group by dwccardnum
),
esl (dwccardnum, eslcount)
as
(
	select asi1.dwccardnum, count(*) as [eslcount]
	from activitysignins asi1
	join activities aa on aa.ID = asi1.activityid

	where aa.nameen in ('English Class 1', 'English Class 2', 'Somos Vecinos')
	and asi1.dateforsignin >= @startdate
	and asi1.dateforsignin <= @enddate
	group by asi1.dwccardnum
),
cardnums (dwccardnum)
as
(
	 select dwccardnum from jobs 
	  union 
	  select dwccardnum from act
	  union
	  select dwccardnum from esl
	
)
select distinct(cn.dwccardnum) [Member number]
, p.fullname [Member name]
, cast(isnull([jobcount],0) as int) as  [Dispatches]
, cast(isnull([actcount],0) as int) as [Activities]
, cast(isnull([eslcount],0) as int) as [ESL]
from cardnums cn 
left join jobs on cn.dwccardnum = jobs.dwccardnum
left join act on jobs.dwccardnum = act.dwccardnum
left join esl on esl.dwccardnum = jobs.dwccardnum
join workers w on cn.dwccardnum = w.dwccardnum
join persons p on w.id = p.id
order by Dispatches desc
