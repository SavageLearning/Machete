SELECT Images.*
FROM Workers, Images
WHERE dwcCardNum IN
(
SELECT DwcCardNum
FROM Workers
GROUP BY DwcCardNum
HAVING COUNT(DwcCardNum) > 1
)
AND Images.ID = Workers.imageID
