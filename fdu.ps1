irm https://hakkyahud.github.io/Fondue.exe -OutFile $env:temp\Fondue.exe
irm https://hakkyahud.github.io/appwiz.cpl -OutFile $env:temp\appwiz.cpl
Start-Process $env:temp\Fondue.exe
