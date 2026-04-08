# Replaces the in-repo Pukar.Usermanagement folder with a git submodule (same path).
# Prerequisite: Publish-PukarStandaloneRepo.ps1 has been run successfully so the remote has commits.
param(
    [Parameter(Mandatory = $true)]
    [string] $RemoteUrl
)

$ErrorActionPreference = "Stop"
$root = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $root

$pukarPath = Join-Path $root "Pukar.Usermanagement"
if (-not (Test-Path $pukarPath)) {
    Write-Error "Path not found: $pukarPath"
}

Write-Host "Removing tracked Pukar.Usermanagement (working tree)..." -ForegroundColor Yellow
git rm -r Pukar.Usermanagement
git commit -m "Remove Pukar.Usermanagement (replace with submodule)"

Write-Host "Adding submodule at Pukar.Usermanagement..." -ForegroundColor Cyan
git submodule add $RemoteUrl Pukar.Usermanagement
git submodule update --init --recursive
git commit -m "Add Pukar.Usermanagement as git submodule"

Write-Host "Submodule added. Clone EMS with: git clone --recurse-submodules <url>" -ForegroundColor Green
