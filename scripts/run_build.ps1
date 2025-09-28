param(
    [string]$ComposeFile = "..\docker-compose.yml",
    [string]$EnvFile = "..\environments\.env",
    [string[]]$Services,
    [switch]$Log
)

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

if (-not [System.IO.Path]::IsPathRooted($ComposeFile)) {
    $ComposeFile = Join-Path $ScriptDir $ComposeFile
}

if (-not [System.IO.Path]::IsPathRooted($EnvFile)) {
    $EnvFile = Join-Path $ScriptDir $EnvFile
}

$LogDir = "logs"
$LogPrefix = "build_logs"
$LogSuffix = ".log"
$NewLogFile = ""

function Show-Help {
    Write-Host "Usage: .\rebuild.ps1 [-ComposeFile file] [-EnvFile file] [-Services service1,service2] [-Log]"
    Write-Host "Example: .\rebuild.ps1 -ComposeFile docker-compose.test.yml -EnvFile .env.test -Services backend,frontend -Log"
    exit
}

# Validate files
if (-not (Test-Path $ComposeFile)) {
    Write-Error "Error: Compose file '$ComposeFile' not found!"
    exit 1
}
if (-not (Test-Path $EnvFile)) {
    Write-Error "Error: Environment file '$EnvFile' not found!"
    exit 1
}

# Logging
if ($Log) {
    if (-not (Test-Path $LogDir)) { New-Item -ItemType Directory -Path $LogDir | Out-Null }
    $last = Get-ChildItem "$LogDir\$LogPrefix*$LogSuffix" -ErrorAction SilentlyContinue |
        ForEach-Object { ($_ -replace ".*$LogPrefix(\d+)$LogSuffix",'$1') } |
        Sort-Object {[int]$_} | Select-Object -Last 1
    if (-not $last) { $next = 1 } else { $next = [int]$last + 1 }
    $NewLogFile = "$LogDir\$LogPrefix$next$LogSuffix"
    Write-Host "Logging output to: $NewLogFile"
}

Write-Host "Using compose file: $ComposeFile"
Write-Host "Using environment file: $EnvFile"

if (-not $Services -or $Services.Count -eq 0) {
    Write-Host "No services specified, rebuilding all."
    $Services = @()
}

# Down
docker-compose.exe -f $ComposeFile down -v --remove-orphans @Services

# Up + Build
if ($Log) {
    docker-compose.exe -f $ComposeFile --env-file $EnvFile up --build -d @Services *>&1 | Tee-Object -FilePath $NewLogFile
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Build successfully. Logs saved to: $NewLogFile"
    } else {
        Write-Error "Build failed! Logs saved to: $NewLogFile"
    }
} else {
    docker-compose.exe -f $ComposeFile --env-file $EnvFile up --build -d @Services
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Build successfully."
    } else {
        Write-Error "Build failed!"
    }
}
