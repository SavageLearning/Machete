Get-Content names.csv | Sort-Object { Get-Random } | out-file -FilePath  .\randomized_names.csv -Encoding ascii
$datelist = @()

foreach ($n in 1..100) { $datelist += (get-date).adddays(-$n).ToString('MM/dd/yyyy') }
1..1000 | Sort-Object { Get-Random } | out-file -FilePath .\randomized_nums.csv -Encoding ascii
 $datelist | Sort-Object { Get-Random } | out-file -FilePath  .\randomized_dates.csv -Encoding ascii