--When updating an ReportDefinition, changing the name is necessary to create a new report. 
--The old one nees to be deleted.

USE [_db]
GO
DELETE FROM [dbo].[ReportDefinitions] WHERE NAME = '{name}'
GO
