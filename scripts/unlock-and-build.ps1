# scripts/unlock-and-build.ps1
param(
    [int]$Pid = 0
)

# 1) tenta parar PID explícito
if ($Pid -gt 0) {
    Write-Host "Tentando parar PID $Pid..."
    Stop-Process -Id $Pid -Force -ErrorAction SilentlyContinue
}

# 2) tenta parar processos chamados AtlasAir ou iisexpress
Get-Process -Name AtlasAir -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process -Name iisexpress -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue

# 3) identifica processos que contenham apphost.exe no CommandLine e os para
Get-CimInstance Win32_Process | Where-Object { $_.CommandLine -and $_.CommandLine -like "*apphost.exe*" } |
    ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }

# 4) remove bin/obj do projeto atual
Write-Host "Removendo bin/obj..."
Remove-Item -Recurse -Force .\bin, .\obj -ErrorAction SilentlyContinue

# 5) reconstrói
Write-Host "Reconstruindo projeto..."
dotnet build