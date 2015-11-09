SELECT ActivitySignins.*
FROM ActivitySignins, Workers
WHERE Workers.DwcCardNum IN
(
SELECT dwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND ActivitySignins.PersonID = Workers.ID
ORDER BY Workers.ID
