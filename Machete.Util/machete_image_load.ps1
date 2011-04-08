#$server = "MACHETE\SQLEXPRESS"
#$database = "MacheteStageTest"
#$username = "trejo"
#$password = "machete"
$server = ".\SQLEXPRESS"$database = "machete"
$query = "INSERT INTO " + $database + ".dbo.Images VALUES (@ImageData, @ImageMimeType, @filename, @Thumbnail, @ThumbnailMimeType, @parenttable, @recordkey,  @datecreated, @dateupdated, @Createdby, @Updatedby)"
$linkpics = "update wkr set  wkr.ImageID = img.ID from " + $database + ".dbo.Images img join " + $database + ".dbo.Workers wkr on (wkr.dwccardnum=img.recordkey)"
$dirpath = "C:\Users\jimmy\Desktop\reportes gabriel\Casa images load\id_photo"
$fileEntries = [IO.Directory]::GetFiles($dirpath); 

$connection=new-object System.Data.SqlClient.SqlConnection
#$connection.ConnectionString="Server={0};Database={1};Integrated Security=False;User ID={2};Password={3}" -f $server,$database,$username,$password
$connection.ConnectionString="Server={0};Database={1};Integrated Security=True" -f $server,$database
$command=new-object system.Data.SqlClient.SqlCommand($query,$connection)
$command.CommandTimeout=120
$connection.Open()

$command.parameters.Add("@parenttable", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.Parameters.Add("@ImageData", [System.Data.SqlDbType]"VarBinary", $buffer.Length) | Out-Null
$command.parameters.Add("@ImageMimeType", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.parameters.Add("@datecreated", [System.Data.SqlDbType]"DateTime")| Out-Null
$command.parameters.Add("@dateupdated", [System.Data.SqlDbType]"DateTime")| Out-Null
$command.parameters.Add("@Createdby", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.parameters.Add("@Updatedby", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.Parameters.Add("@Thumbnail", [System.Data.SqlDbType]"VarBinary", $buffer.Length)| Out-Null
$command.parameters.Add("@ThumbnailMimeType", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.parameters.Add("@recordkey", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
$command.parameters.Add("@filename", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null

foreach($fullName in $fileEntries) 
{ 
    [Console]::WriteLine($fullName)
    if ($fullName -match '.*\\(?<recordkey>\d{0,})\.JPG') {
        $recordkey = $matches.recordkey
        $fullName -match '.*\\(?<filename>\d{0,}\.JPG)'
        $filename = $matches.filename
        [Console]::WriteLine($recordkey); 
        #
        $fs = new-object System.IO.FileStream($fullname, [System.IO.FileMode]'Open', [System.IO.FileAccess]'Read')
        $buffer = new-object byte[] -ArgumentList $fs.Length
        $fs.Read($buffer, 0, $buffer.Length)
        $fs.Close()
        #        
        $command.Parameters["@ImageData"].Value = $buffer
        $command.Parameters["@ImageMimeType"].Value = "image/jpeg"
        $command.Parameters["@datecreated"].Value = Get-Date
        $command.Parameters["@dateupdated"].Value = Get-Date
        $command.Parameters["@Createdby"].Value = "PowerShellImport"
        $command.Parameters["@Updatedby"].Value = "PowerShellImport"
        $command.Parameters["@Thumbnail"].Value = [DBNull]::Value
        $command.Parameters["@ThumbnailMimeType"].Value = [DBNull]::Value
        $command.Parameters["@recordkey"].Value = $recordkey
        $command.Parameters["@filename"].Value = $fileName
        $command.Parameters["@parenttable"].Value = "Workers"
        $command.ExecuteNonQuery()
    }
} 

$connection.Close()