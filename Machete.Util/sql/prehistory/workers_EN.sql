USE [machete]
GO

/****** Object:  View [db_datareader].[workers_EN]    Script Date: 06/19/2011 21:02:10 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[db_datareader].[workers_EN]'))
DROP VIEW [db_datareader].[workers_EN]
GO

USE [machete]
GO

/****** Object:  View [db_datareader].[workers_EN]    Script Date: 06/19/2011 21:02:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [db_datareader].[workers_EN]
AS
SELECT     wo.ID, p.firstname1, p.firstname2, p.lastname1, p.lastname2, p.address1, p.address2, p.city, p.state, p.zipcode, p.phone,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = p.gender)) AS Gender, wo.dateOfMembership, wo.dateOfBirth, 
                      CASE p.[active] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS activeworker,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.RaceID)) AS Race, wo.raceother, wo.height, wo.weight, wo.englishlevelID AS englishlevel, 
                      CASE [recentarrival] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS recentarrival, wo.dateinUSA, wo.dateinseattle, 
                      CASE [disabled] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS disabled, wo.disabilitydesc,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.maritalstatus)) AS maritalstatus, CASE [livewithchildren] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS livewithchildren, 
                      wo.numofchildren,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.incomeID)) AS incomeID, CASE [livealone] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS livealone, wo.emcontUSAname, 
                      wo.emcontUSArelation, wo.emcontUSAphone, wo.dwccardnum,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.neighborhoodID)) AS neighborhood, CASE [immigrantrefugee] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS immigrantrefugee,
                          (SELECT     text_EN
                            FROM          dbo.Lookups AS l
                            WHERE      (ID = wo.countryoforiginID)) AS countryoforigin, wo.emcontoriginname, wo.emcontoriginrelation, wo.emcontoriginphone, wo.memberexpirationdate, 
                      CASE [driverslicense] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS driverslicense, wo.licenseexpirationdate, 
                      CASE [carinsurance] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS carinsurance, wo.insuranceexpiration, wo.ImageID, wo.skill1, wo.skill2, 
                      wo.skill3, wo.datecreated, wo.dateupdated, wo.Createdby, wo.Updatedby
FROM         dbo.Workers AS wo INNER JOIN
                      dbo.Persons AS p ON wo.ID = p.ID


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
         Begin Table = "wo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 125
               Right = 433
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
' , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workers_EN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'db_datareader', @level1type=N'VIEW',@level1name=N'workers_EN'
GO

