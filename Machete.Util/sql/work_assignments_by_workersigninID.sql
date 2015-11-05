SELECT WorkAssignments.*
FROM WorkAssignments, Workers, WorkerSignins
WHERE Workers.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND WorkerSignins.WorkerID = Workers.ID
AND WorkerSignins.ID = WorkAssignments.workerSigninID
ORDER BY Workers.ID
