declare @startDate DateTime = '1/1/2016';
declare @endDate DateTime = '1/1/2017';
declare @dwccardnum integer = 30533;

SELECT     
l.text_en AS eventType, 
CONVERT(VARCHAR(11), dateFrom, 100) AS evDateFrom, 
CONVERT(VARCHAR(11), dateTo, 100) AS evDateTo, 
notes, 
ev.datecreated AS evDateCreated, 
ev.dateupdated AS evDateUpdated, 
ev.Createdby AS evCreatedby, 
ev.Updatedby AS evUpdatedby
FROM         Events AS ev
join workers w on (ev.PersonID = w.id)
join lookups l on (ev.eventType = l.id)
WHERE     (w.dwccardnum = @dwccardnum)
