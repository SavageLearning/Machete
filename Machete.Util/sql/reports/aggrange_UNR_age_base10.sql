declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';

with demos_age (age_range, ordinal)
as 
(
	SELECT
	CASE
		WHEN age < 20 THEN 'Under_20'
		WHEN age BETWEEN 20 and 29 THEN '20-29'
		WHEN age BETWEEN 30 and 39 THEN '30-39'
		WHEN age BETWEEN 40 and 49 THEN '40-49'
		WHEN age BETWEEN 50 and 59 THEN '50-59'
		WHEN age BETWEEN 60 and 69 THEN '60-69'
		WHEN age BETWEEN 70 and 79 THEN '70-79'
		WHEN age >= 80 THEN 'Over_80'
		WHEN age IS NULL THEN 'NULL'
	END as age_range,
		CASE
		WHEN age < 20 THEN 1
		WHEN age BETWEEN 20 and 29 THEN 2
		WHEN age BETWEEN 30 and 39 THEN 3
		WHEN age BETWEEN 40 and 49 THEN 4
		WHEN age BETWEEN 50 and 59 THEN 5
		WHEN age BETWEEN 60 and 69 THEN 6
		WHEN age BETWEEN 70 and 79 THEN 7
		WHEN age >= 80 THEN 8
		WHEN age IS NULL THEN 9
	END as ordinal
	FROM (
		select w.id, w.age, w.dateofbirth
		from (
			select ID,(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),dateOfBirth,112))/10000 AS age, dateofbirth
			from Workers
		) as W
		join workersignins wsi on wsi.workerID = W.ID
		WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
		group by w.id, w.age, w.dateofbirth
	) as a
)
select 
	convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByAgeGroupBase10-' + convert(varchar(10), min(age_range)) as id,	
	age_range as [Age range], 
	count(*) as [Count]
from demos_age 
group by age_range, ordinal
order by ordinal
