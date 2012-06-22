select wid,
	   aid,
	   asiid, 
	   groupingint, 
	   dwccardnum, 
	   firstname1 + ' ' + lastname1 as member, d
	   uration as minutes, 
	   row,
	   Grouping, 
	   datefrom 
from (
	select wid,
		   aid,
		   asiid, 
		   duration, 
		   'After ' + testname as Grouping, 
		   repname.row, 
		   repname.datefrom, 
		   groupingint 
	from 
	(
		SELECT WID, 
				SUM(duration) AS duration,
				classdate, 
				r.AID, 
				r.ASIID,
				(	
					select count(ww.id) colgroup
					from workers ww
					join dbo.events ee on (ww.id = ee.personID)
					join lookups ll on (ee.eventtype = ll.id)
					where ll.subcategory = 'test' and
						  ww.id = WID and
						  ee.dateFrom < classdate		
				) as groupingINT
		FROM (SELECT w.ID WID, 
					 a.id aid, 
					 asi.id asiid,
					 a.dateStart classdate,
					 /** Use Activity type to get English Lookup name **/
					 (SELECT text_EN FROM Lookups AS Lookups_1 WHERE (ID = a.type)) AS classtype,
					 /** calculate the DURATION **/ 
					 (DATEPART(hour, a.dateEnd) * 60 + DATEPART(minute, a.dateEnd)) - 
								(DATEPART(hour, a.dateStart) * 60 + DATEPART(minute, a.dateStart)) AS duration
			   FROM Workers AS w INNER JOIN /** **/
				    ActivitySignins AS asi ON w.ID = asi.personID INNER JOIN
				    Activities AS a ON a.ID = asi.ActivityID 
			   ) AS r
		GROUP BY WID, classdate, classtype, aid, asiid
	) rr
	left join 
	(
		select row_number() over (partition by w.id order by e.datefrom) row, 
				w.id wwid, 
				(select text_EN from lookups lll where lll.id = e.eventtype) testname, 
				e.dateFrom
		from workers w 
		join dbo.events e on (w.id = e.personID)
		join lookups l on (e.eventtype = l.id)
		where l.subcategory = 'test'
		union all
		(
			select 0 as row, id as id, 'initiation' as testname, dateofbirth as datefrom
			from workers 
		)
	) as repname on (rr.groupingINT = repname.row and rr.wid = repname.wwid)
) report
join persons ppp on (report.wid = ppp.id)
join workers www on (report.wid = www.id)
order by dwccardnum, aid, asiid
