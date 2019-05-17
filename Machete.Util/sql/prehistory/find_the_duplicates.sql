SELECT *
FROM Persons, Workers
WHERE Persons.ID = Workers.ID
AND Workers.dwccardnum in (
SELECT DwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
ORDER BY Workers.dwccardnum




