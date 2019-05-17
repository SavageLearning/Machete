SELECT WorkerRequests.*
FROM WorkerRequests, Workers
WHERE Workers.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND WorkerRequests.workerID = Workers.ID
ORDER BY Workers.ID

