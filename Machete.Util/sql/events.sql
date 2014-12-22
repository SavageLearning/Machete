SELECT Events.*
FROM Events, Workers
WHERE Workers.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND Events.PersonID = Workers.ID
ORDER BY Workers.ID
