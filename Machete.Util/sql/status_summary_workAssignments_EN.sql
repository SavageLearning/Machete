USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[workAssignments_status_summary_EN]    Script Date: 08/22/2011 15:22:13 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[db_datareader].[workAssignments_status_summary_EN]'))
DROP VIEW [db_datareader].[workAssignments_status_summary_EN]
GO

USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[workAssignments_status_summary_EN]    Script Date: 08/22/2011 15:22:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [db_datareader].[workAssignments_status_summary_EN]
AS
SELECT     TOP (100) PERCENT startdate, SUM(CASE status WHEN 43 THEN countof ELSE '' END) AS [Pending Assignments], 
                      SUM(CASE status WHEN 42 THEN countof ELSE '' END) AS [Active Assignments], SUM(CASE status WHEN 44 THEN countof ELSE '' END) 
                      AS [Completed Assignments], SUM(CASE status WHEN 45 THEN countof ELSE '' END) AS [Cancelled Assignments], 
                      SUM(CASE status WHEN 46 THEN countof ELSE '' END) AS [Expired Assignments]
FROM         (SELECT     startdate, status, COUNT(ID) AS countof
                       FROM          (SELECT     CONVERT(char(10), wo.dateTimeofWork, 126) AS startdate, was.ID, wo.status
                                               FROM          dbo.WorkAssignments AS was INNER JOIN
                                                                      dbo.WorkOrders AS wo ON wo.ID = was.workOrderID) AS waa
                       GROUP BY startdate, status) AS summed
GROUP BY startdate
ORDER BY startdate DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "summed"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 99
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workAssignments_status_summary_EN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workAssignments_status_summary_EN'
GO

