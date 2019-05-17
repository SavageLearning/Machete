-- Take the output of the command
-- echo "'$(dotnet ef migrations list --project Machete.Web | awk 'FNR == 3')', '$(dotnet ef --version | awk 'FNR == 2')'"
-- and put it into 'VALUES' below

TRUNCATE TABLE machete_db.dbo.__EFMigrationsHistory
GO

INSERT INTO machete_db.dbo.__EFMigrationsHistory (MigrationId, ProductVersion)
VALUES ('20190511230648_InitialCreate', '2.2.4-servicing-10062')
GO
