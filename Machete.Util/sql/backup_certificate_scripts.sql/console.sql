USE master
GO
--
-- ------------- --
-- GENERATE KEYS --
-- ------------- --
--
ALTER SERVICE MASTER KEY FORCE REGENERATE;
--
BACKUP SERVICE MASTER KEY TO FILE = '/var/opt/mssql/certs/servicemasterkey.smk'
    ENCRYPTION BY PASSWORD='';
GO
--
CREATE MASTER KEY
    ENCRYPTION BY PASSWORD='';
GO
--
BACKUP MASTER KEY TO FILE = '/var/opt/mssql/certs/masterkey.key'
    ENCRYPTION BY PASSWORD='';
GO
--
CREATE CERTIFICATE sqlserver_backup_cert
    WITH SUBJECT = 'sqlserver_backup_cert'
GO
--
BACKUP CERTIFICATE sqlserver_backup_cert TO FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.cer'
    WITH PRIVATE KEY ( FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.key' ,
    ENCRYPTION BY PASSWORD = '')
GO
--
-- ------------- --
-- RESTORE  KEYS --
-- ------------- --
--
USE master
GO
--
--DROP CERTIFICATE sqlserver_backup_cert
--GO
--
--ALTER SERVICE MASTER KEY FORCE REGENERATE
--GO
--
RESTORE SERVICE MASTER KEY FROM FILE = '/var/opt/mssql/certs/servicemasterkey.smk'
    DECRYPTION BY PASSWORD = ''
    --
    FORCE
    --
GO
--
DROP MASTER KEY
GO
--CREATE MASTER KEY ENCRYPTION BY PASSWORD = ''
RESTORE MASTER KEY FROM FILE = '/var/opt/mssql/certs/masterkey.key'
    DECRYPTION BY PASSWORD = ''
    ENCRYPTION BY PASSWORD  = ''
GO
--
OPEN MASTER KEY DECRYPTION BY PASSWORD = '';
--
CREATE CERTIFICATE sqlserver_backup_cert FROM FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.crt'
  WITH PRIVATE KEY (
    FILE = '/var/opt/mssql/certs/sqlserver_backup_cert.key',
    DECRYPTION BY PASSWORD = ''
  )
GO
--
CLOSE MASTER KEY;
