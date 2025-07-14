#
# PanoramicData.ConsoleExtensions NuGet Publisher
# 
# This script automates the process of building and publishing the 
# PanoramicData.ConsoleExtensions package to NuGet.org
#
# Prerequisites:
# - Valid NuGet API key stored in 'nuget.key' file
# - PowerShell 5.0 or higher
# - .NET SDK installed
#
# Usage:
#   .\publish.ps1                           # Default: Release configuration
#   .\publish.ps1 -Configuration Debug      # Custom configuration
#   .\publish.ps1 -KeyFile "mykey.txt"      # Custom key file
#

# PowerShell script to publish PanoramicData.ConsoleExtensions to NuGet
param(
    [string]$Configuration = "Release",
    [string]$KeyFile = "nuget.key"
)

Write-Host "PanoramicData.ConsoleExtensions NuGet Publisher" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""

# Check if the API key file exists
$keyFilePath = Join-Path $PSScriptRoot $KeyFile

if (-not (Test-Path $keyFilePath)) {
    Write-Host "ERROR: NuGet API key file not found!" -ForegroundColor Red
    Write-Host ""
    Write-Host "To publish this package, you need to:" -ForegroundColor Yellow
    Write-Host "1. Obtain a NuGet API key from https://www.nuget.org/account/apikeys" -ForegroundColor Yellow
    Write-Host "2. Create a file named '$KeyFile' in the solution root directory" -ForegroundColor Yellow
    Write-Host "3. Put your API key (and only the API key) in that file" -ForegroundColor Yellow
    Write-Host "4. Make sure the file is listed in .gitignore to keep your key secure" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Example key file content:" -ForegroundColor Cyan
    Write-Host "oy2abc123def456ghi789jkl012mno345pqr678st" -ForegroundColor Gray
    Write-Host ""
    Write-Host "The key file should contain ONLY the API key with no extra whitespace." -ForegroundColor Yellow
    exit 1
}

# Read the API key
$apiKey = Get-Content $keyFilePath -Raw
$apiKey = $apiKey.Trim()

if ([string]::IsNullOrWhiteSpace($apiKey)) {
    Write-Host "ERROR: The API key file '$KeyFile' is empty!" -ForegroundColor Red
    Write-Host "Please add your NuGet API key to the file." -ForegroundColor Yellow
    exit 1
}

# Check if the key file contains placeholder content (comments or instructions)
if ($apiKey.StartsWith("#") -or $apiKey.Contains("This file should contain") -or $apiKey.Contains("Replace this comment")) {
    Write-Host "ERROR: The API key file '$KeyFile' contains placeholder content!" -ForegroundColor Red
    Write-Host ""
    Write-Host "To publish this package, you need to:" -ForegroundColor Yellow
    Write-Host "1. Obtain a NuGet API key from https://www.nuget.org/account/apikeys" -ForegroundColor Yellow
    Write-Host "2. Replace the contents of '$KeyFile' with your actual API key" -ForegroundColor Yellow
    Write-Host "3. The file should contain ONLY the API key with no comments or extra text" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Example key file content:" -ForegroundColor Cyan
    Write-Host "oy2abc123def456ghi789jkl012mno345pqr678st" -ForegroundColor Gray
    Write-Host ""
    Write-Host "The key file should contain ONLY the API key with no extra whitespace." -ForegroundColor Yellow
    exit 1
}

# Validate key format (NuGet keys are typically 40+ alphanumeric characters)
if ($apiKey.Length -lt 40 -or $apiKey -notmatch '^[a-zA-Z0-9]+$') {
    Write-Host "ERROR: The API key format appears to be invalid!" -ForegroundColor Red
    Write-Host "NuGet API keys are typically 40+ alphanumeric characters." -ForegroundColor Yellow
    Write-Host "Current key length: $($apiKey.Length) characters" -ForegroundColor Yellow
    exit 1
}

Write-Host "API key file found: $keyFilePath" -ForegroundColor Green
Write-Host "API key length: $($apiKey.Length) characters" -ForegroundColor Green
Write-Host ""

# Build the solution
Write-Host "Building solution in $Configuration configuration..." -ForegroundColor Yellow
dotnet build --configuration $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""

# Pack the project
Write-Host "Creating NuGet package..." -ForegroundColor Yellow
dotnet pack .\PanoramicData.ConsoleExtensions\PanoramicData.ConsoleExtensions.csproj --configuration $Configuration --no-build
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Packaging failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Package created successfully!" -ForegroundColor Green
Write-Host ""

# Find the generated package
$packagePath = Get-ChildItem -Path ".\PanoramicData.ConsoleExtensions\bin\$Configuration" -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if (-not $packagePath) {
    Write-Host "ERROR: Could not find generated NuGet package!" -ForegroundColor Red
    exit 1
}

Write-Host "Found package: $($packagePath.FullName)" -ForegroundColor Green
Write-Host "Package size: $([math]::Round($packagePath.Length / 1KB, 2)) KB" -ForegroundColor Green
Write-Host ""

# Confirm publication
Write-Host "Ready to publish package to NuGet.org" -ForegroundColor Yellow
Write-Host "Package: $($packagePath.Name)" -ForegroundColor Cyan
$confirmation = Read-Host "Do you want to proceed? (y/N)"

if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
    Write-Host "Publication cancelled." -ForegroundColor Yellow
    exit 0
}

# Publish the package
Write-Host ""
Write-Host "Publishing package to NuGet.org..." -ForegroundColor Yellow
dotnet nuget push $packagePath.FullName --api-key $apiKey --source https://api.nuget.org/v3/index.json

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Package published successfully!" -ForegroundColor Green
    Write-Host "It may take a few minutes to appear in search results." -ForegroundColor Yellow
} else {
    Write-Host ""
    Write-Host "ERROR: Failed to publish package!" -ForegroundColor Red
    Write-Host "Check the error messages above for details." -ForegroundColor Yellow
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Publication completed!" -ForegroundColor Green
