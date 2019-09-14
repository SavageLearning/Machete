-- Take the output of the command
-- echo "'$(dotnet ef migrations list --project Machete.Web | awk 'FNR == 3')', '$(dotnet ef --version | awk 'FNR == 2')'"
-- and put it into 'VALUES' below

--USE ____________
--    your db here


use machete_db
go

create table [__EFMigrationsHistory]
(
  MigrationId    nvarchar(150) not null
    constraint PK___EFMigrationsHistory
      primary key,
  ProductVersion nvarchar(32)  not null
)
go

INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
  VALUES ('20190522011430_Prehistoric', '2.2.4-servicing-10062')
GO

UPDATE dbo.AspNetUsers
   SET NormalizedUserName = UPPER(UserName)
GO


