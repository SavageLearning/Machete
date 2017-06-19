declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'

SELECT zipcode, gender, dateOfBirth, disabled, RaceID, text_en, englishlevelID, MIN(dateforsignin) as firstdateforsignin, Ws.dwccardnum, Ps.ID, homeless, incomeID, livewithchildren, livealone,
immigrantrefugee
FROM dbo.Workers Ws
JOIN dbo.Persons Ps ON Ws.ID = Ps.ID
JOIN dbo.Lookups Ls ON Ls.ID = Ws.RaceID 
JOIN dbo.WorkerSignins WSIs ON Ws.ID = WSIs.WorkerID
WHERE dateforsignin >= @startDate and dateforsignin <= @endDate

GROUP BY Ws.dwccardnum, Ps.ID, zipcode, gender, maritalstatus, livewithchildren, numofchildren, dateOfBirth, disabled, RaceID, text_en, englishlevelID, 
homeless, incomeID, livewithchildren, livealone, immigrantrefugee

ORDER BY Ws.dwccardnum


select * from reportdefinitions