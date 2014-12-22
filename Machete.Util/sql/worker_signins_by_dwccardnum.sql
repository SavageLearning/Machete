SELECT *
FROM WorkerSignins
WHERE WorkerSignins.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
ORDER BY DwcCardNum
