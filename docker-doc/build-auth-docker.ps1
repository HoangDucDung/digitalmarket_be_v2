#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build Docker image cho Digitalmarket Auth API

.DESCRIPTION
    Script này hỗ trợ build, run, và quản lý Docker image
    - Tự động detect root directory (nơi có .sln file)
    - Build với optimized multi-stage
    - Tuỳ chọn run container ngay sau khi build

.PARAMETER ImageTag
    Tag cho image (default: latest)
    
.PARAMETER Run
    Chạy container ngay sau khi build

.PARAMETER Port
    Port mapping (default: 8081)

.PARAMETER ConfigPath
    Đường dẫn tới Config folder (default: ./Config)

.EXAMPLE
    .\build-auth-docker.ps1
    .\build-auth-docker.ps1 -ImageTag "1.0.0" -Run
    .\build-auth-docker.ps1 -ImageTag "latest" -Run -Port 8081
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$ImageTag = "latest",
    
    [Parameter(Mandatory = $false)]
    [switch]$Run,
    
    [Parameter(Mandatory = $false)]
    [int]$Port = 8081,
    
    [Parameter(Mandatory = $false)]
    [string]$ConfigPath = "./Config"
)

# Colors
$Green = "`e[32m"
$Yellow = "`e[33m"
$Red = "`e[31m"
$Blue = "`e[34m"
$Reset = "`e[0m"

function Write-Success {
    Write-Host "$Green✓ $($args -join ' ')$Reset"
}

function Write-Info {
    Write-Host "$Yellow→ $($args -join ' ')$Reset"
}

function Write-Error-Custom {
    Write-Host "$Red✗ $($args -join ' ')$Reset"
}

function Write-Header {
    Write-Host "$Blue▌ $($args -join ' ')$Reset"
}

# Get root directory (where .sln file is)
$RootDir = Split-Path -Parent $PSScriptRoot
if (-not (Test-Path "$RootDir/digitalmarket_be_v2.sln")) {
    Write-Error-Custom "Not in solution root directory. Cannot find digitalmarket_be_v2.sln"
    exit 1
}

$ImageName = "digitalmarket-auth"
$FullImageName = "$ImageName`:$ImageTag"
$DockerfilePath = "Presentation/API/Digitalmarket.Controller.Auth/Dockerfile"

Write-Header "Docker Build - Digitalmarket Auth API"
Write-Host ""
Write-Info "Building image: $FullImageName"
Write-Info "Dockerfile: $DockerfilePath"
Write-Info "Build context: $RootDir"
Write-Host ""

# Build
Write-Info "Running docker build..."
docker build `
    -f $DockerfilePath `
    -t $FullImageName `
    . 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Error-Custom "Docker build failed!"
    exit 1
}

Write-Success "Image built successfully: $FullImageName"
Write-Host ""

# Show image info
Write-Info "Image details:"
docker images | Select-String $ImageName | Select-Object -First 1

# Run container if requested
if ($Run) {
    Write-Host ""
    $ContainerName = "auth-api-$(Get-Date -Format yyyyMMdd-HHmmss)"
    $ConfigAbsPath = (Resolve-Path $ConfigPath -ErrorAction SilentlyContinue).Path
    
    Write-Info "Starting container: $ContainerName"
    
    if ($ConfigAbsPath) {
        Write-Info "Mounting Config from: $ConfigAbsPath"
        docker run -d `
            -p "$Port`:8080" `
            -v "$ConfigAbsPath`:/Config" `
            --name $ContainerName `
            $FullImageName 2>&1
    } else {
        Write-Info "Config folder not found. Running without volume mount."
        docker run -d `
            -p "$Port`:8080" `
            --name $ContainerName `
            $FullImageName 2>&1
    }
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Custom "Failed to start container!"
        exit 1
    }
    
    Write-Success "Container started: $ContainerName"
    Write-Info "Wait 3 seconds for startup..."
    Start-Sleep -Seconds 3
    
    Write-Info "Container logs:"
    docker logs $ContainerName | Select-Object -First 20
    
    Write-Info "Container status:"
    docker ps | Select-String $ContainerName | Format-Table
    
    Write-Success "Auth API ready at: http://localhost:$Port"
    Write-Info "To stop: docker stop $ContainerName"
    Write-Info "To remove: docker rm $ContainerName"
    Write-Info "To view logs: docker logs $ContainerName"
}

Write-Success "Done!"
