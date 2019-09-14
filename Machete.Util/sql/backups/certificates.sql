
-- -- troubleshooting -- --
--DROP CERTIFICATE sqlserver_backup_cert
--DROP MASTER KEY
--ALTER SERVICE MASTER KEY FORCE REGENERATE
--GO

USE master;
GO
CREATE MASTER KEY ENCRYPTION BY PASSWORD = ''; -- a password goes here.
GO

CREATE CERTIFICATE sqlserver_backup_cert
WITH SUBJECT = 'sqlserver_backup_cert'

BACKUP CERTIFICATE sqlserver_backup_cert
    TO FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.cer'
    WITH PRIVATE KEY (FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.key', ENCRYPTION BY PASSWORD = '') -- a password goes here, too.

-- -- only for testing -- --
--BACKUP DATABASE voz_db TO DISK = N'/var/opt/mssql/backup/voz_db.bak' WITH NOFORMAT, NOINIT, NAME = 'voz_db-full', SKIP, NOREWIND, NOUNLOAD, STATS = 10
