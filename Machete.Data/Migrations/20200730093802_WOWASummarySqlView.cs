using Machete.Data.Initialize;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Machete.Data.Migrations
{
    public partial class WOWASummarySqlView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  VIEW [dbo].[View_WOWASummary] AS
WITH cte_offSet (OFFSET)
AS (
	SELECT DATEDIFF(HOUR, GETUTCDATE() AT TIME ZONE(SELECT value FROM dbo.Configs WHERE [key] = 'MicrosoftTimeZoneIndex'), GETUTCDATE()) AS [offset]
	)
,cte_client_dates (clientDate, WOID)
AS (
	SELECT DATEADD(HOUR, (SELECT offset FROM cte_offSet), dateTimeofWork) AS [clientDate]
		, ID as [WOID]
	FROM WorkOrders
	WHERE DATEDIFF(dd, dateTimeofWork, CURRENT_TIMESTAMP) < 180
	)
select [date], cast([date] as date) as sortableDate, [weekday], [PendingWO],[PendingWA],[ActiveWO],[ActiveWA],[CompletedWO],[CompletedWA],[CancelledWO],[CancelledWA],[ExpiredWO],[ExpiredWA]
from
(
    select CONVERT(varchar, cd.clientDate, 101) as [date], DATENAME(dw, cd.clientDate) as [weekday], CONCAT([statusEN], 'WO') [status], count(*) as [count]
    from dbo.workorders as wo
	join cte_client_dates as cd on cd.WOID = wo.ID
    where DATEDIFF(dd, dateTimeofWork, CURRENT_TIMESTAMP) < 180
    group by CONVERT(varchar, cd.clientDate, 101),  DATENAME(dw, cd.clientDate), CONCAT([statusEN], 'WO')
    union
    select CONVERT(varchar, cd.clientDate, 101) as [date], DATENAME(dw, cd.clientDate) as [weekday], CONCAT([statusEN], 'WA') status, count(*) as [count]
    from dbo.workorders as wo
    join dbo.WorkAssignments as wa on wo.id = wa.workOrderID
	join cte_client_dates as cd on cd.WOID = wo.ID
    where DATEDIFF(dd, dateTimeofWork, CURRENT_TIMESTAMP) < 180
    group by CONVERT(varchar, cd.clientDate, 101),  DATENAME(dw, cd.clientDate), CONCAT([statusEN], 'WA')
) as source
PIVOT
(
    sum([count])
    for [status] in ([PendingWO],[PendingWA],[ActiveWO],[ActiveWA],[CompletedWO],[CompletedWA],[CancelledWO],[CancelledWA],[ExpiredWO],[ExpiredWA])
) as WOWASummary

GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
