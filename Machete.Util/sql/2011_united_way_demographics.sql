USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[2011_united_way_demographics]    Script Date: 07/17/2011 17:51:36 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[db_datareader].[2011_united_way_demographics]'))
DROP VIEW [db_datareader].[2011_united_way_demographics]
GO

USE [macheteStageProd]
GO

/****** Object:  View [db_datareader].[2011_united_way_demographics]    Script Date: 07/17/2011 17:51:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [db_datareader].[2011_united_way_demographics]
AS
SELECT     TOP (100) PERCENT db_datareader.united_way_demographics.dwccardnum, db_datareader.united_way_demographics.city, 
                      db_datareader.united_way_demographics.zipcode, db_datareader.united_way_demographics.homeless, db_datareader.united_way_demographics.zipunknown, 
                      db_datareader.united_way_demographics.gender, db_datareader.united_way_demographics.AfricanAmerican, db_datareader.united_way_demographics.Asian, 
                      db_datareader.united_way_demographics.PacificHawaiian, db_datareader.united_way_demographics.Hispanic, 
                      db_datareader.united_way_demographics.NativeAmerican, db_datareader.united_way_demographics.WhiteCaucasian, 
                      db_datareader.united_way_demographics.Other, db_datareader.united_way_demographics.limitedenglish, db_datareader.united_way_demographics.newarrival, 
                      db_datareader.united_way_demographics.disability, db_datareader.united_way_demographics.age, db_datareader.united_way_demographics.numofchildren
FROM         db_datareader.united_way_demographics INNER JOIN
                          (SELECT     dwccardnum
                            FROM          dbo.WorkerSignins
                            WHERE      (dateforsignin >= '2010-07-01') AND (dateforsignin < '2011-07-01')
                            GROUP BY dwccardnum) AS uniquelist ON db_datareader.united_way_demographics.dwccardnum = uniquelist.dwccardnum
ORDER BY db_datareader.united_way_demographics.dwccardnum

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[26] 4[15] 2[19] 3) )"
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
         Begin Table = "united_way_demographics (db_datareader)"
            Begin Extent = 
               Top = 5
               Left = 288
               Bottom = 202
               Right = 446
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "uniquelist"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 69
               Right = 189
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
      Begin ColumnWidths = 19
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
' , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'2011_united_way_demographics'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'2011_united_way_demographics'
GO

