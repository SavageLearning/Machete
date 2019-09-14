
-- ## 1
--
-- This first section is a query to drop the `discriminator` column from the AspNetUserRoles view. When the view was created, discriminator existed (purpose unknown), now it does not.
--
--USE [_db]
-- GO

-- SET ANSI_NULLS ON
-- GO

-- SET QUOTED_IDENTIFIER ON
-- GO

-- ALTER VIEW [db_datareader].[View_AspNetUserRoles]
-- AS
-- SELECT AspNetUserRoles.userID, AspNetUsers.userName, AspNetUsers.mobileAlias, AspNetUsers.isAnonymous, AspNetUsers.lastActivityDate, AspNetUsers.email, AspNetUsers.loweredEmail, AspNetUsers.loweredUserName, AspNetUsers.passwordQuestion, AspNetUsers.isApproved, AspNetUsers.isLockedOut, AspNetUsers.lastLoginDate, AspNetUsers.lastPasswordChangedDate, AspNetUsers.lastLockoutDate, AspNetUsers.failedPasswordAttemptCount, AspNetUsers.failedPasswordAttemptWindowStart, AspNetUsers.failedPasswordAnswerAttemptCount, AspNetUsers.failedPasswordAnswerAttemptWindowStart, AspNetUsers.comment, AspNetUserRoles.roleID, AspNetRoles.name AS roleName
-- FROM AspNetUserRoles
-- LEFT JOIN AspNetUsers
-- ON AspNetUserRoles.userID = AspNetUsers.ID
-- LEFT JOIN AspNetRoles
-- ON AspNetUserRoles.roleID = AspNetRoles.ID
-- GO
-- 
-- ## 1

-- ## 2
--
-- This second section adds the "Prehistoric" migration to the database. Do not add both migrations. It will not work.
-- This schema includes all migrations up to and including 1.13.x.
--
USE [_db]
GO
create table [__EFMigrationsHistory]
(
   MigrationId    nvarchar(150) not null
       constraint PK___EFMigrationsHistory
           primary key,
   ProductVersion nvarchar(32)  not null
)
go
INSERT INTO [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion)
VALUES ('20190522011430_Prehistoric',   '2.2.4-servicing-10062')
GO
--
-- Create the Center table (if it does not exist), which I deleted on most (but not all) of the databases sometime during April panic.
--USE [_db]
--GO
create table Center
(
    ID                        int          not null
        primary key,
    Name                      nvarchar(50) not null,
    Description               nvarchar(200),
    Address1                  nvarchar(50),
    Address2                  nvarchar(50),
    City                      nvarchar(25),
    State                     nvarchar(2),
    zipcode                   nvarchar(10),
    phone                     varchar(12),
    Center_contact_firstname1 varchar(50),
    Center_contact_lastname1  varchar(50)
)
go
-- add the LegacyPasswordHash column if it is missing
--USE [_db]
--GO
alter table dbo.AspNetUsers
add  [LegacyPasswordHash] nvarchar(max)
GO
-- ## 2


-- ## 3
--
-- These scripts are run after the migration takes place.
--
USE [_db]
GO
update dbo.AspNetUsers
    set normalizedusername = UPPER(UserName)
update dbo.AspNetUsers
    set normalizedemail = UPPER(Email)
GO
--USE [_db]
--GO
update dbo.AspNetRoles set NormalizedName = upper(Name)
GO
-- ## 3
