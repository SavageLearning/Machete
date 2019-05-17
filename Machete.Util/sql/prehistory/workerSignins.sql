USE [machete]
GO

/****** Object:  View [db_datareader].[workOrders_EN]    Script Date: 06/20/2011 10:48:03 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[db_datareader].[workOrders_EN]'))
DROP VIEW [db_datareader].[workOrders_EN]
GO

USE [machete]
GO

/****** Object:  View [db_datareader].[workOrders_EN]    Script Date: 06/20/2011 10:48:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [db_datareader].[workOrders_EN]
AS
SELECT     ID, EmployerID, contactName,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.status)) AS Status, workSiteAddress1, workSiteAddress2, city, state, phone, zipcode,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.typeOfWorkID)) AS TypeOfWorkOrder, CASE [englishRequired] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS EnglishRequired,
                       englishRequiredNote, CASE [lunchSupplied] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS LunchSupplied, 
                      CASE [permanentPlacement] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS permanentPlacement,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.transportMethodID)) AS TransportMethod, transportFee, transportFeeExtra, description, dateTimeofWork, 
                      CASE [timeFlexible] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS FlexibleStateTime, datecreated, dateupdated, Createdby, Updatedby
FROM         dbo.WorkOrders AS wo

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[24] 2[34] 3) )"
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
         Begin Table = "wo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 228
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
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
      Begin ColumnWidths = 11
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
' , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workOrders_EN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workOrders_EN'
GO

