USE [master]

--RESTORE FILELISTONLY FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-2013-11-18.BAK' 
RESTORE DATABASE [aspnetdb] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb_log.ldf', 
REPLACE, STATS = 10

--RESTORE FILELISTONLY FROM DISK =  N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-concord-2013-11-18.BAK' 
RESTORE DATABASE [aspnetdb-concord] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-concord-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-concord.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-concord_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-elcentro] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-elcentro-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-elcentro.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-elcentro_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-graton] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-graton-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-graton.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-graton_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-hayward] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-hayward-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-hayward.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-hayward_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-mtnview] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-mtnview-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-mtnview.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-mtnview_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-pas] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-pas-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-pas.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-pas_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-santacruz] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-santacruz-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-santacruz.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-santacruz_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-sf] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-sf-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-sf.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-sf_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [aspnetdb-test] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-test-2013-11-18.BAK' 
WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-test.mdf', 
MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-test_log.ldf', 
REPLACE, STATS = 10

--RESTORE DATABASE [machete] 
--FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-2013-11-18.BAK' 
--WITH MOVE 'machete' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete.mdf', 
--MOVE 'machete_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete_log.ldf', 
--REPLACE, STATS = 10


RESTORE DATABASE [machete-concord] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-concord-2013-11-18.BAK' 
WITH MOVE 'machete-concord' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-concord.mdf', 
MOVE 'machete-concord_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-concord_log.ldf', 
REPLACE, STATS = 10

--RESTORE FILELISTONLY FROM DISK =  N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-elcentro-2013-11-18.BAK' 
RESTORE DATABASE [machete-elcentro] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-elcentro-2013-11-18.BAK' 
WITH MOVE 'machete' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-elcentro.mdf', 
MOVE 'machete_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-elcentro_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-graton] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-graton-2013-11-18.BAK' 
WITH MOVE 'machete-graton' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-graton.mdf', 
MOVE 'machete-graton_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-graton_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-hayward] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-hayward-2013-11-18.BAK' 
WITH MOVE 'machete-hayward' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-hayward.mdf', 
MOVE 'machete-hayward_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-hayward_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-mtnview] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-mtnview-2013-11-18.BAK' 
WITH MOVE 'machete-mtnview' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-mtnview.mdf', 
MOVE 'machete-mtnview_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-mtnview_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-pas] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-pas-2013-11-18.BAK' 
WITH MOVE 'machete-pas' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-pas.mdf', 
MOVE 'machete-pas_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-pas_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-santacruz] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-santacruz-2013-11-18.BAK' 
WITH MOVE 'machete-santacruz' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-santacruz.mdf', 
MOVE 'machete-santacruz_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-santacruz_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-sf] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-sf-2013-11-18.BAK' 
WITH MOVE 'machete-sf' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-sf.mdf', 
MOVE 'machete-sf_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-sf_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-test] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-test-2013-11-18.BAK' 
WITH MOVE 'machete-test' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-test.mdf', 
MOVE 'machete-test_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-test_log.ldf', 
REPLACE, STATS = 10

--RESTORE FILELISTONLY FROM DISK =  N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-wjp-2013-11-18.BAK' 
RESTORE DATABASE [machete-wjp]
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-wjp-2013-11-18.BAK' 
WITH MOVE 'machete' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-wjp.mdf', 
MOVE 'machete_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-wjp_log.ldf', 
REPLACE, STATS = 10

--RESTORE DATABASE [machetelog] 
--FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-2013-11-18.BAK' 
--WITH MOVE 'machetelog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog.mdf', 
--MOVE 'machetelog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog_log.ldf', 
--REPLACE, STATS = 10

--RESTORE FILELISTONLY FROM DISK =  N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-concord-2013-11-18.BAK' 

RESTORE DATABASE [machetelog-concord] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-concord-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-concord.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-concord_log.ldf', 
REPLACE, STATS = 10

--RESTORE FILELISTONLY FROM DISK =  N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-elcentro-2013-11-18.BAK' 

RESTORE DATABASE [machetelog-elcentro] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-elcentro-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-elcentro.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-elcentro_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-graton] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-graton-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-graton.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-graton_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-hayward] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-hayward-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-hayward.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-hayward_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-mtnview] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-mtnview-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-mtnview.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-mtnview_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-pas] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-pas-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-pas.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-pas_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-santacruz] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-santacruz-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-santacruz.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-santacruz_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-sf] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-sf-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-sf.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-sf_log.ldf', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-test] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-test-2013-11-18.BAK' 
WITH MOVE 'ELMAHlog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-test.mdf', 
MOVE 'ELMAHlog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-test_log.ldf', 
REPLACE, STATS = 10

USE [master]

--RESTORE FILELISTONLY FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-macheteStageProd.bak'
RESTORE DATABASE [machete-casa] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-macheteStageProd.bak' WITH  FILE = 1,  
MOVE 'macheteStageProd' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-casa.mdf',  
MOVE N'macheteStageProd_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete-casa_log.ldf',  
NOUNLOAD,  STATS = 5

--RESTORE FILELISTONLY FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-machetelogStageProd.bak'
RESTORE DATABASE [machetelog-casa] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-machetelogStageProd.bak' WITH  FILE = 1,  
MOVE N'ELMAHlog' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-casa.mdf',  
MOVE N'ELMAHlog_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog-casa_log.ldf',  
NOUNLOAD,  STATS = 5

--RESTORE FILELISTONLY FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-aspnetdbStageProd.bak'
RESTORE DATABASE [aspnetdb-casa] 
FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\SQL2013-11-18-machete-aspnetdbStageProd.bak' WITH  FILE = 1,  
MOVE N'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-casa.mdf',  
MOVE N'ASPNETDB_TMP_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb-casa_log.ldf',  
NOUNLOAD,  STATS = 5

GO


