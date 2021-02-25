

--
-- Sometimes we need to give users access to a test database but want to 
-- limit their permissions to only that DB instance.
--
-- We can execute the following script(s) in order to accomplish this goal.
--

Use volunteer_db;
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'volunteerApp')
BEGIN
    create login [volunteerApp] with password = '';
    CREATE USER [volunteerApp] FOR LOGIN [volunteerApp] ;
    EXEC sp_addrolemember N'db_owner', N'volunteerApp';
END;
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'volunteer_reader')
BEGIN
    create login [volunteer_reader] with password = '';
    CREATE USER [volunteer_reader] FOR LOGIN [volunteer_reader]
    EXEC sp_addrolemember N'db_datareader', N'volunteer_reader'
END;
go


--

-- show databases on server
SELECT name, database_id, create_date
FROM sys.databases ;
go;
-- show database row & log file-paths on disk
SELECT
    db.name AS DBName,
    type_desc AS FileType,
    Physical_Name AS Location
FROM
    sys.master_files mf
INNER JOIN
    sys.databases db ON db.database_id = mf.database_id;
--- Show Database roles and members
SELECT DP1.name AS DatabaseRoleName,
   isnull (DP2.name, 'No members') AS DatabaseUserName
 FROM sys.database_role_members AS DRM
 RIGHT OUTER JOIN sys.database_principals AS DP1
   ON DRM.role_principal_id = DP1.principal_id
 LEFT OUTER JOIN sys.database_principals AS DP2
   ON DRM.member_principal_id = DP2.principal_id
WHERE DP1.type = 'R'
ORDER BY DP1.name;
--- show all tables in a database
SELECT
  *
FROM
SYSOBJECTS
WHERE
  xtype = 'U';
GO

--
--
