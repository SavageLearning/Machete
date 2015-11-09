SELECT WorkAssignments.*
FROM WorkAssignments, Workers
WHERE Workers.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND WorkAssignments.workerAssignedID = Workers.ID
ORDER BY Workers.ID
 
