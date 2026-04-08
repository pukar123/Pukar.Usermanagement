# Pushes the git branch created by: git subtree split -P Pukar.Usermanagement -b pukar-um-standalone
# Prerequisite: create an EMPTY GitHub (or other) repository first, then pass its URL.
param(
    [Parameter(Mandatory = $true)]
    [string] $RemoteUrl
)

$ErrorActionPreference = "Stop"
Set-Location (Join-Path $PSScriptRoot "..")

if (-not (git rev-parse --verify pukar-um-standalone 2>$null)) {
    Write-Host "Branch pukar-um-standalone not found. Creating with subtree split..." -ForegroundColor Yellow
    git subtree split -P Pukar.Usermanagement -b pukar-um-standalone
}

Write-Host "Pushing pukar-um-standalone -> $RemoteUrl (main)..." -ForegroundColor Cyan
git push $RemoteUrl pukar-um-standalone:main

Write-Host "Done. Next: run Complete-PukarSubmodule.ps1 with the same -RemoteUrl." -ForegroundColor Green
