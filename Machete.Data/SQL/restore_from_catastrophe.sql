--ORIGINAL FORMAT:

--this command can be used to get the filelist for any file:
--RESTORE FILELISTONLY FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-elcentro.BAK' 

--there are three databases to be restored
--RESTORE DATABASE [aspnetdb] 
--FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\aspnetdb-' + @todaystring + '.BAK' 
--WITH MOVE 'ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb.mdf', 
--MOVE 'ASPNETDB_TMP_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\aspnetdb_log.ldf', 
--REPLACE, STATS = 10

--RESTORE DATABASE [machete] 
--FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machete-' + @todaystring + '.BAK' 
--WITH MOVE 'machete' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete.mdf', 
--MOVE 'machete_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machete_log.ldf', 
--REPLACE, STATS = 10

--RESTORE DATABASE [machetelog] 
--FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\restore\machetelog-' + @todaystring + '.BAK' 
--WITH MOVE 'machetelog' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog.mdf', 
--MOVE 'machetelog_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\machetelog_log.ldf', 
--REPLACE, STATS = 10

USE [master]

DECLARE @restore varchar(max)
DECLARE @i int = 0
DECLARE @centername varchar(max)
DECLARE @today DATETIME = SYSDATETIME()
DECLARE @todaystring varchar(10) = CONVERT(VARCHAR(10), @today, 121)
DECLARE @ourpath nvarchar(max) = 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\'

WHILE @i < 14 
BEGIN
  SET @i = @i + 1
  set @centername = 
    case
 	  when @i = 1 then 'bosco'
 	  when @i = 2 then 'casa'
 	  when @i = 3 then 'concord'
 	  when @i = 4 then 'elcentro'
 	  when @i = 5 then 'graton'
  	  when @i = 6 then 'hayward'
	  when @i = 7 then 'mtnview'
	  when @i = 8 then 'pas'
	  when @i = 9 then 'pomona'
	  when @i = 10 then 'santacruz'
	  when @i = 11 then 'sf'
	  when @i = 12 then 'test'
	  when @i = 13 then 'voz'
	  when @i = 14 then 'wjp'
	 else '' end

--this part does not need to be automated
--RESTORE FILELISTONLY FROM DISK =  N'' + @ourpath + 'restore\aspnetdb-' + @centername + '.BAK''
set @restore = 
'RESTORE DATABASE [aspnetdb-' + @centername + '] 
FROM DISK = N''' + @ourpath + 'restore\aspnetdb-' + @centername + '-' + @todaystring + '.BAK'' 
WITH MOVE ''ASPNETDB_896bf2f83b89468c8cd8c0e78f49b94d_DAT'' TO ''' + @ourpath + 'aspnetdb-' + @centername + '.mdf'', 
MOVE ''ASPNETDB_TMP_log'' TO ''' + @ourpath + 'aspnetdb-' + @centername + '_log.ldf'', 
REPLACE, STATS = 10

RESTORE DATABASE [machete-' + @centername + '] 
FROM DISK = N''' + @ourpath + 'restore\machete-' + @centername + '-' + @todaystring + '.BAK'' 
WITH MOVE ''machete-' + @centername + ''' TO ''' + @ourpath + 'machete-' + @centername + '.mdf'', 
MOVE ''machete-' + @centername + '_log'' TO ''' + @ourpath + 'machete-' + @centername + '_log.ldf'', 
REPLACE, STATS = 10

RESTORE DATABASE [machetelog-' + @centername + '] 
FROM DISK = N''' + @ourpath + 'restore\machetelog-' + @centername + '-' + @todaystring + '.BAK'' 
WITH MOVE ''ELMAHlog'' TO ''' + @ourpath + 'machetelog-' + @centername + '.mdf'', 
MOVE ''ELMAHlog_log'' TO ''' + @ourpath + 'machetelog-' + @centername + '_log.ldf'', 
REPLACE, STATS = 10'

EXEC(@restore)

END