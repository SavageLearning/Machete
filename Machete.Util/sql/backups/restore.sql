-- https://ola.hallengren.com/sql-server-backup.html
EXECUTE dbo.DatabaseBackup
@Databases = 'USER_DATABASES',
@Directory = '/var/opt/mssql/backups',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@Encrypt = 'Y',
@EncryptionAlgorithm = 'AES_256',
@ServerCertificate = 'sqlserver_backup_cert'

-- https://docs.microsoft.com/en-us/sql/linux/tutorial-restore-backup-in-sql-server-container?view=sql-server-2017#copy-a-backup-file-into-the-container
RESTORE FILELISTONLY FROM DISK = '/var/opt/mssql/backups/f34bbb807ec0/casa_db/FULL/f34bbb807ec0_casa_db_FULL_20190826_195736.bak'

RESTORE DATABASE casa_db
    FROM DISK = '/var/opt/mssql/backups/f34bbb807ec0/casa_db/FULL/f34bbb807ec0_casa_db_FULL_20190826_195736.bak'
    WITH REPLACE -- < -- < --
