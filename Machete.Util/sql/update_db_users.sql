-- script for updating the readonly user password or access.

-- has to run in the `master` scope
USE MASTER
GO
-- get rid of old login and create new
ALTER LOGIN readonlylogin WITH NAME = old_readonlylogin2;
ALTER LOGIN old_readonlylogin2 DISABLE
CREATE LOGIN readonlylogin WITH PASSWORD='fakezz';
ALTER LOGIN readonlylogin ENABLE
GO

--run the script below for each db
USE machete_db
GO

ALTER USER readonlyuser WITH NAME = old_readonlyuser2;
REVOKE CONNECT FROM old_readonlyuser40
CREATE USER readonlyuser FROM LOGIN readonlylogin;
--apply the readonly permissions
EXEC sp_addrolemember 'db_datareader', 'readonlyuser';
GO
