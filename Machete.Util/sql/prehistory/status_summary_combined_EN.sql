USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[status_summary_combined_EN]    Script Date: 07/08/2011 00:48:41 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[db_datareader].[status_summary_combined_EN]'))
DROP VIEW [db_datareader].[status_summary_combined_EN]
GO

USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[status_summary_combined_EN]    Script Date: 07/08/2011 00:48:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [db_datareader].[status_summary_combined_EN]
AS
SELECT     db_datareader.workOrders_status_summary_EN.startdate, db_datareader.workOrders_status_summary_EN.weekday, 
                      db_datareader.workOrders_status_summary_EN.[Pending Orders], db_datareader.workAssignments_status_summary_EN.[Pending Assignments], 
                      db_datareader.workOrders_status_summary_EN.[Active Orders], db_datareader.workAssignments_status_summary_EN.[Active Assignments], 
                      db_datareader.workOrders_status_summary_EN.[Completed Orders], db_datareader.workAssignments_status_summary_EN.[Completed Assignments], 
                      db_datareader.workOrders_status_summary_EN.[Cancelled Orders], db_datareader.workAssignments_status_summary_EN.[Cancelled Assignments], 
                      db_datareader.workOrders_status_summary_EN.[Expired Orders], db_datareader.workAssignments_status_summary_EN.[Expired Assignments]
FROM         db_datareader.workOrders_status_summary_EN INNER JOIN
                      db_datareader.workAssignments_status_summary_EN ON 
                      db_datareader.workOrders_status_summary_EN.startdate = db_datareader.workAssignments_status_summary_EN.startdate


GO


