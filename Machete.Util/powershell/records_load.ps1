$database = "macheteStageProd"
#$database = "machete"
$server = ".\SQLEXPRESS"
$person_query = "INSERT INTO " + $database + ".dbo.Persons VALUES (DEFAULT, @active, @firstname1, @firstname2, @lastname1, @lastname2, @address1, @address2, @city, @state, @zipcode, @phone, @gender, @genderother, @datecreated, @dateupdated, @Createdby, @Updatedby); SET @PKID = SCOPE_IDENTITY();"
$worker_query = "INSERT INTO " + $database + ".dbo.Workers VALUES (@ID, @dateOfMembership, @dateOfBirth, @w_active, @RaceID, @raceother, @height, @weight, @englishlevelID, @recentarrival, @dateinUSA, @dateinseattle, @disabled, @disabilitydesc, @maritalstatus, @livewithchildren, @numofchildren, @incomeID, @livealone, @emcontUSAname, @emcontUSArelation, @emcontUSAphone, @dwccardnum, @neighborhoodID, @immigrantrefugee, @countryoforigin, @emcontoriginname, @emcontoriginrelation, @emcontoriginphone, @memberexpirationdate, @driverslicense, @licenseexpirationdate, @carinsurance, @insuranceexpiration, @ImageID, @w_datecreated, @w_dateupdated, @w_Createdby, @w_Updatedby)"
#$worker_query = "INSERT INTO " + $database + ".dbo.Workers VALUES (@ID, @w_active, @RaceID, @raceother, @height, @weight, @englishlevelID, @recentarrival, @dateinUSA, @dateinseattle, @disabled, @disabilitydesc, @maritalstatus, @livewithchildren, @numofchildren, @incomeID, @livealone, @emcontUSAname, @emcontUSArelation, @emcontUSAphone, @dwccardnum, @neighborhoodID, @immigrantrefugee, @countryoforigin, @emcontoriginname, @emcontoriginrelation, @emcontoriginphone, @memberexpirationdate, @driverslicense, @licenseexpirationdate, @carinsurance, @insuranceexpiration, @ImageID, @w_datecreated, @w_dateupdated, @w_Createdby, @w_Updatedby)"
$file = Select-FileDialog
$records = Import-Csv $file -Delimiter ","


$connection=new-object System.Data.SqlClient.SqlConnection
$connection.ConnectionString="Server={0};Database={1};Integrated Security=True" -f $server,$database
$insert_person=new-object system.Data.SqlClient.SqlCommand($person_query,$connection)
$insert_person.CommandTimeout=120
#$insert_person.parameters.Add("@timestamp", [System.Data.SqlDbType]"Timestamp") | Out-Null
$insert_person.parameters.Add("@active", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_person.parameters.Add("@firstname1", [System.Data.SqlDbType]"NVarChar", 50)  | Out-Null
$insert_person.parameters.Add("@firstname2", [System.Data.SqlDbType]"NVarChar", 50) | Out-Null
$insert_person.parameters.Add("@lastname1", [System.Data.SqlDbType]"NVarChar", 50) | Out-Null
$insert_person.parameters.Add("@lastname2", [System.Data.SqlDbType]"NVarChar", 50)| Out-Null
$insert_person.parameters.Add("@address1", [System.Data.SqlDbType]"NVarChar", 50) | Out-Null
$insert_person.parameters.Add("@address2", [System.Data.SqlDbType]"NVarChar", 50) | Out-Null
$insert_person.parameters.Add("@city", [System.Data.SqlDbType]"NVarChar", 25) | Out-Null
$insert_person.parameters.Add("@state", [System.Data.SqlDbType]"NVarChar", 2) | Out-Null
$insert_person.parameters.Add("@zipcode", [System.Data.SqlDbType]"NVarChar", 10) | Out-Null
$insert_person.parameters.Add("@phone", [System.Data.SqlDbType]"NVarChar", 12) | Out-Null
$insert_person.parameters.Add("@gender", [System.Data.SqlDbType]"Int")| Out-Null
$insert_person.parameters.Add("@genderother", [System.Data.SqlDbType]"NVarChar", 20) | Out-Null      
$insert_person.parameters.Add("@datecreated", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_person.parameters.Add("@dateupdated", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_person.parameters.Add("@Createdby", [System.Data.SqlDbType]"NVarChar", 30) | Out-Null
$insert_person.parameters.Add("@Updatedby", [System.Data.SqlDbType]"NVarChar", 30)| Out-Null
$insert_person.Parameters.Add("@PKID", [System.Data.SqlDbType]"Int").Direction = [System.Data.ParameterDirection]::Output
$insert_worker=new-object system.Data.SqlClient.SqlCommand($worker_query,$connection)
$insert_worker.CommandTimeout=120

$insert_worker.parameters.Add("@ID", [System.Data.SqlDbType]"Int") | Out-Null
$insert_worker.parameters.Add("@dateOfMembership", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@dateOfBirth", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@w_active", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@RaceID", [System.Data.SqlDbType]"Int") | Out-Null
$insert_worker.parameters.Add("@raceother", [System.Data.SqlDbType]"NVarChar", 20)  | Out-Null
$insert_worker.parameters.Add("@height", [System.Data.SqlDbType]"NVarChar", 50)  | Out-Null
$insert_worker.parameters.Add("@weight", [System.Data.SqlDbType]"NVarChar", 10)  | Out-Null
$insert_worker.parameters.Add("@englishlevelID", [System.Data.SqlDbType]"TinyInt") | Out-Null
$insert_worker.parameters.Add("@recentarrival", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@dateinUSA", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@dateinseattle", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@disabled", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@disabilitydesc", [System.Data.SqlDbType]"NVarChar", 50)  | Out-Null
$insert_worker.parameters.Add("@maritalstatus", [System.Data.SqlDbType]"NVarChar", 1)  | Out-Null
$insert_worker.parameters.Add("@livewithchildren", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@numofchildren", [System.Data.SqlDbType]"TinyInt") | Out-Null
$insert_worker.parameters.Add("@incomeID", [System.Data.SqlDbType]"TinyInt") | Out-Null
$insert_worker.parameters.Add("@livealone", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@emcontUSAname", [System.Data.SqlDbType]"NVarChar", 50)  | Out-Null
$insert_worker.parameters.Add("@emcontUSArelation", [System.Data.SqlDbType]"NVarChar", 30)  | Out-Null
$insert_worker.parameters.Add("@emcontUSAphone", [System.Data.SqlDbType]"NVarChar", 14)  | Out-Null
$insert_worker.parameters.Add("@dwccardnum", [System.Data.SqlDbType]"Int") | Out-Null
$insert_worker.parameters.Add("@neighborhoodID", [System.Data.SqlDbType]"TinyInt") | Out-Null
$insert_worker.parameters.Add("@immigrantrefugee", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@countryoforigin", [System.Data.SqlDbType]"NVarChar", 20)  | Out-Null
$insert_worker.parameters.Add("@emcontoriginname", [System.Data.SqlDbType]"NVarChar", 50)  | Out-Null
$insert_worker.parameters.Add("@emcontoriginrelation", [System.Data.SqlDbType]"NVarChar", 30)  | Out-Null
$insert_worker.parameters.Add("@emcontoriginphone", [System.Data.SqlDbType]"NVarChar", 14)  | Out-Null
$insert_worker.parameters.Add("@memberexpirationdate", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@driverslicense", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@licenseexpirationdate", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@carinsurance", [System.Data.SqlDbType]"Bit") | Out-Null
$insert_worker.parameters.Add("@insuranceexpiration", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@ImageID", [System.Data.SqlDbType]"int") | Out-Null
$insert_worker.parameters.Add("@w_datecreated", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@w_dateupdated", [System.Data.SqlDbType]"DateTime") | Out-Null
$insert_worker.parameters.Add("@w_Createdby", [System.Data.SqlDbType]"NVarChar", 30) | Out-Null
$insert_worker.parameters.Add("@w_Updatedby", [System.Data.SqlDbType]"NVarChar", 30)| Out-Null

$connection.Open()

$records | Foreach-Object {
    $row = $_
    $date = Get-Date
    $data_expires = [DateTime] $row.data_expires
    [Console]::WriteLine($row)
    #$insert_person.Parameters["@timestamp"].Value = [BitConverter]::GetBytes(0)
    if ($data_expires -lt $date) { $active = 0 } else { $active = 1 }
    $insert_person.Parameters["@active"].Value = $active        
    $insert_person.Parameters["@firstname1"].Value = $row.name_id
    $insert_person.Parameters["@firstname2"].Value = [DBNull]::Value
    $insert_person.Parameters["@lastname1"].Value = $row.c_last_name
    $insert_person.Parameters["@lastname2"].Value = [DBNull]::Value        
    $insert_person.Parameters["@address1"].Value = [DBNull]::Value        
    $insert_person.Parameters["@address2"].Value = [DBNull]::Value
    $insert_person.Parameters["@city"].Value = [DBNull]::Value
    $insert_person.Parameters["@state"].Value = [DBNull]::Value
    $insert_person.Parameters["@zipcode"].Value = [DBNull]::Value
    $insert_person.Parameters["@phone"].Value = [DBNull]::Value
    if ($row.id_sex -eq "M") {
        $insert_person.Parameters["@gender"].Value = 38
    }
    else
    {
        $insert_person.Parameters["@gender"].Value = 39
    }
    $insert_person.Parameters["@genderother"].Value = [DBNull]::Value
    $insert_person.Parameters["@datecreated"].Value = Get-Date
    $insert_person.Parameters["@dateupdated"].Value = Get-Date
    $insert_person.Parameters["@Createdby"].Value = "PowerShellImport"
    $insert_person.Parameters["@Updatedby"].Value = "PowerShellImport"       
    if ($insert_person.ExecuteNonQuery()) {
        $ID = $insert_person.Parameters["@PKID"].Value
    } else {
        exit
    }
    $insert_worker.Parameters["@ID"].Value = $ID
    $insert_worker.Parameters["@dateOfMembership"].Value = $row.date_birthdate   
    $insert_worker.Parameters["@dateOfBirth"].Value = $row.Fecha_de_Ingreso   
    $insert_worker.Parameters["@w_active"].Value = $active
    $insert_worker.Parameters["@RaceID"].Value = 5 
    $insert_worker.Parameters["@raceother"].Value = [DBNull]::Value
    $insert_worker.Parameters["@height"].Value = $row.c_height
    $insert_worker.Parameters["@weight"].Value = $row.id_weight
    $insert_worker.Parameters["@englishlevelID"].Value = 1
    $insert_worker.Parameters["@recentarrival"].Value = 0
    $insert_worker.Parameters["@dateinUSA"].Value = Get-Date
    $insert_worker.Parameters["@dateinseattle"].Value = Get-Date
    $insert_worker.Parameters["@disabled"].Value = 0
    $insert_worker.Parameters["@disabilitydesc"].Value = [DBNull]::Value
    $insert_worker.Parameters["@maritalstatus"].Value = 33
    $insert_worker.Parameters["@livewithchildren"].Value = 0
    $insert_worker.Parameters["@numofchildren"].Value = 0
    $insert_worker.Parameters["@incomeID"].Value = 17
    $insert_worker.Parameters["@livealone"].Value = 1
    $insert_worker.Parameters["@emcontUSAname"].Value = [DBNull]::Value
    $insert_worker.Parameters["@emcontUSArelation"].Value = [DBNull]::Value
    $insert_worker.Parameters["@emcontUSAphone"].Value = [DBNull]::Value
    $insert_worker.Parameters["@dwccardnum"].Value = $row.cod_id
    $insert_worker.Parameters["@neighborhoodID"].Value = 11
    $insert_worker.Parameters["@immigrantrefugee"].Value = 0
    $insert_worker.Parameters["@countryoforigin"].Value = 48
    $insert_worker.Parameters["@emcontoriginname"].Value = [DBNull]::Value
    $insert_worker.Parameters["@emcontoriginrelation"].Value = [DBNull]::Value
    $insert_worker.Parameters["@emcontoriginphone"].Value = [DBNull]::Value
    $insert_worker.Parameters["@memberexpirationdate"].Value = $data_expires
    $insert_worker.Parameters["@driverslicense"].Value = 0
    $insert_worker.Parameters["@licenseexpirationdate"].Value = [DBNull]::Value
    $insert_worker.Parameters["@carinsurance"].Value = 0
    $insert_worker.Parameters["@insuranceexpiration"].Value = [DBNull]::Value
    $insert_worker.Parameters["@ImageID"].Value = [DBNull]::Value
    $insert_worker.Parameters["@w_datecreated"].Value = Get-Date
    $insert_worker.Parameters["@w_dateupdated"].Value = Get-Date
    $insert_worker.Parameters["@w_Createdby"].Value = "PowerShellImport"
    $insert_worker.Parameters["@w_Updatedby"].Value = "PowerShellImport"

        if ($insert_worker.ExecuteNonQuery()) {
        [Console]::WriteLine("Worker & Person record added for ID " + $ID + " with DWC# " + $row.Cod)
    } else {
        exit
    }  
        
}
$connection.Close() 


function Select-FileDialog
{
	param([string]$Title,[string]$Directory,[string]$Filter="All Files (*.*)|*.*")
	[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms") | Out-Null
	$objForm = New-Object System.Windows.Forms.OpenFileDialog
	$objForm.InitialDirectory = $Directory
	$objForm.Filter = $Filter
	$objForm.Title = $Title
	$Show = $objForm.ShowDialog()
	If ($Show -eq "OK")
	{
		Return $objForm.FileName
	}
	Else
	{
		Write-Error "Operation cancelled by user."
	}
}