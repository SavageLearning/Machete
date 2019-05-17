declare @startDate DateTime = '1/1/2015';
declare @endDate DateTime = '1/1/2017';
declare @dwccardnum integer = 30533;


select
	convert(varchar(7), signindate, 102) as YearMonth,
	case when (sum(cast([1] as int))) > 0 then 'X' else '' end as '1',
	case when (sum(cast([2] as int))) > 0 then 'X' else '' end as '2',
	case when (sum(cast([3] as int))) > 0 then 'X' else '' end as '3',
	case when (sum(cast([4] as int))) > 0 then 'X' else '' end as '4',
	case when (sum(cast([5] as int))) > 0 then 'X' else '' end as '5',
	case when (sum(cast([6] as int))) > 0 then 'X' else '' end as '6',
	case when (sum(cast([7] as int))) > 0 then 'X' else '' end as '7',
	case when (sum(cast([8] as int))) > 0 then 'X' else '' end as '8',
	case when (sum(cast([9] as int))) > 0 then 'X' else '' end as '9',
	case when (sum(cast([10] as int))) > 0 then 'X' else '' end as '10',
	case when (sum(cast([11] as int))) > 0 then 'X' else '' end as '11',
	case when (sum(cast([12] as int))) > 0 then 'X' else '' end as '12',
	case when (sum(cast([13] as int))) > 0 then 'X' else '' end as '13',
	case when (sum(cast([14] as int))) > 0 then 'X' else '' end as '14',
	case when (sum(cast([15] as int))) > 0 then 'X' else '' end as '15',
	case when (sum(cast([16] as int))) > 0 then 'X' else '' end as '16',
	case when (sum(cast([17] as int))) > 0 then 'X' else '' end as '17',
	case when (sum(cast([18] as int))) > 0 then 'X' else '' end as '18',
	case when (sum(cast([19] as int))) > 0 then 'X' else '' end as '19',
	case when (sum(cast([20] as int))) > 0 then 'X' else '' end as '20',
	case when (sum(cast([21] as int))) > 0 then 'X' else '' end as '21',
	case when (sum(cast([22] as int))) > 0 then 'X' else '' end as '22',
	case when (sum(cast([23] as int))) > 0 then 'X' else '' end as '23',
	case when (sum(cast([24] as int))) > 0 then 'X' else '' end as '24',
	case when (sum(cast([25] as int))) > 0 then 'X' else '' end as '25',
	case when (sum(cast([26] as int))) > 0 then 'X' else '' end as '26',
	case when (sum(cast([27] as int))) > 0 then 'X' else '' end as '27',
	case when (sum(cast([28] as int))) > 0 then 'X' else '' end as '28',
	case when (sum(cast([29] as int))) > 0 then 'X' else '' end as '29',
	case when (sum(cast([30] as int))) > 0 then 'X' else '' end as '30',
	case when (sum(cast([31] as int))) > 0 then 'X' else '' end as '31'
from
(
	SELECT
	  wsi.WorkerID as workerID
	  ,wsi.dateforsignin as signindate
	  ,day(wsi.dateforsignin) as [day]
	FROM
	  WorkerSignins wsi
	where dwccardnum = @dwccardnum
	and wsi.dateforsignin >= @startDate
	and wsi.dateforsignin <= @endDate
) as foo
pivot
(
count (workerID)
for [day] in 

( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31] )
) as pvt
group by convert(varchar(7), signindate, 102)