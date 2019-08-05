SELECT WorkerSignins.*
FROM Workers, WorkerSignins
WHERE Workers.ID = WorkerSignins.WorkerID
AND Workers.dwccardnum in (
SELECT DwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
ORDER BY Workers.dwccardnum
