SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[View_WOWASummary] AS
select [date],[weekday], [PendingWO],[PendingWA],[ActiveWO],[ActiveWA],[CompletedWO],[CompletedWA],[CancelledWO],[CancelledWA],[ExpiredWO],[ExpiredWA]
from
(
    select CONVERT(varchar, dateTimeofWork, 101) as [date], DATENAME(dw, dateTimeofWork) as [weekday], CONCAT([statusEN], 'WO') [status], count(*) as [count]
    from dbo.workorders as wo
    where DATEDIFF(dd, dateTimeofWork, CURRENT_TIMESTAMP) < 180
    group by CONVERT(varchar, dateTimeofWork, 101),  DATENAME(dw, dateTimeofWork), CONCAT([statusEN], 'WO')
    union
    select CONVERT(varchar, dateTimeofWork, 101) as [date], DATENAME(dw, dateTimeofWork) as [weekday], CONCAT([statusEN], 'WA') status, count(*) as [count]
    from dbo.workorders as wo
    join dbo.WorkAssignments as wa on wo.id = wa.workOrderID
    where DATEDIFF(dd, dateTimeofWork, CURRENT_TIMESTAMP) < 180
    group by CONVERT(varchar, dateTimeofWork, 101),  DATENAME(dw, dateTimeofWork), CONCAT([statusEN], 'WA')
) as source
PIVOT
(
    sum([count])
    for [status] in ([PendingWO],[PendingWA],[ActiveWO],[ActiveWA],[CompletedWO],[CompletedWA],[CancelledWO],[CancelledWA],[ExpiredWO],[ExpiredWA])
) as WOWASummary

GO
