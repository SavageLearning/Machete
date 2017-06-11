declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';
with demos_age (age_range, ordinal)
as 
(
	SELECT
	CASE
		WHEN age < 14 THEN 'Under_14'
		WHEN age BETWEEN 14 and 17 THEN '14-17'
		WHEN age BETWEEN 18 and 24 THEN '18-24'
		WHEN age BETWEEN 25 and 34 THEN '25-34'
		WHEN age BETWEEN 35 and 54 THEN '35-54'
		WHEN age BETWEEN 55 and 59 THEN '55-59'
		WHEN age BETWEEN 60 and 64 THEN '60-64'
		WHEN age BETWEEN 65 and 74 THEN '65-74'
		WHEN age BETWEEN 75 and 84 THEN '75-84'
		WHEN age >= 85 THEN 'Over_85'
		WHEN age IS NULL THEN 'NULL'
	END as age_range,
		CASE
		WHEN age < 14 THEN 1
		WHEN age BETWEEN 14 and 17 THEN 2
		WHEN age BETWEEN 18 and 24 THEN 3
		WHEN age BETWEEN 25 and 34 THEN 4
		WHEN age BETWEEN 35 and 54 THEN 5
		WHEN age BETWEEN 55 and 59 THEN 6
		WHEN age BETWEEN 60 and 64 THEN 7
		WHEN age BETWEEN 65 and 74 THEN 8
		WHEN age BETWEEN 75 and 84 THEN 9
		WHEN age >= 85 THEN 10
		WHEN age IS NULL THEN 11
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
	convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByAgeGroupUnitedWay-' + convert(varchar(10), min(age_range)) as id,	
	age_range as [Age range], 
	count(*) as [Count]
from demos_age 
group by age_range, ordinal
order by ordinal