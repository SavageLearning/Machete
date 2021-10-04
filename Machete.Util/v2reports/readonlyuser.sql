/***
  Use case: to update the test environment databses once to allow for
  V2 sql reports.
  The DBs had an existing readonlyuser, but with different access.
**/

USE machete_db
GO
ALTER USER readonlyuser WITH NAME = old_readonlyuser2;
REVOKE CONNECT FROM old_readonlyuser2
ALTER LOGIN readonlylogin WITH NAME = old_readonlylogin2;
ALTER LOGIN old_readonlylogin2 DISABLE

USE machete_db
GO

CREATE LOGIN readonlylogin WITH PASSWORD='XXXXXXXXXX';
CREATE USER readonlyuser FROM LOGIN readonlylogin;
EXEC sp_addrolemember 'db_datareader', 'readonlyuser';
