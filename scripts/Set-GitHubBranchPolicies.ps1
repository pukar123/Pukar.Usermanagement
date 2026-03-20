<#
.SYNOPSIS
  Sets default branch to 'development' and classic branch protection on development, pre-prod, prod.

.DESCRIPTION
  Requires a GitHub PAT with admin rights on the repo (repo scope: Administration read/write, or use a fine-grained token with Rules and Administration).

  Usage:
    $env:GITHUB_TOKEN = "ghp_..."   # or pass -Token
    .\scripts\Set-GitHubBranchPolicies.ps1

  Or:
    .\scripts\Set-GitHubBranchPolicies.ps1 -Token ghp_xxx -Owner pukar123 -Repo EmployeeManagementSystem

  If you prefer the GitHub UI, see CONTRIBUTING.md → GitHub one-time setup.
#>
[CmdletBinding()]
param(
    [string] $Owner = "pukar123",
    [string] $Repo = "EmployeeManagementSystem",
    [string] $Token = $(if ($env:GITHUB_TOKEN) { $env:GITHUB_TOKEN } elseif ($env:GH_TOKEN) { $env:GH_TOKEN } else { "" })
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($Token)) {
    Write-Error "No token. Set GITHUB_TOKEN or pass -Token. See CONTRIBUTING.md for manual steps."
}

$base = "https://api.github.com"
$headers = @{
    Authorization                = "Bearer $Token"
    Accept                       = "application/vnd.github+json"
    "X-GitHub-Api-Version"       = "2022-11-28"
    "User-Agent"                 = "EMS-Set-GitHubBranchPolicies"
}

function Invoke-GitHubJson {
    param(
        [string] $Method,
        [string] $Uri,
        [object] $Body = $null
    )
    $params = @{
        Method      = $Method
        Uri         = $Uri
        Headers     = $headers
        ContentType = "application/json"
    }
    if ($null -ne $Body) {
        $params.Body = ($Body | ConvertTo-Json -Depth 10 -Compress)
    }
    Invoke-RestMethod @params
}

Write-Host "Setting default branch to 'development'..."
Invoke-GitHubJson -Method PATCH -Uri "$base/repos/$Owner/$Repo" -Body @{ default_branch = "development" } | Out-Null

# Classic branch protection: PR required before merge, no force-push, no branch deletion
$protectionJson = @'
{
  "required_status_checks": { "strict": false, "contexts": [] },
  "enforce_admins": false,
  "required_pull_request_reviews": {
    "dismiss_stale_reviews": false,
    "require_code_owner_reviews": false,
    "require_last_push_approval": false,
    "required_approving_review_count": 0
  },
  "restrictions": null,
  "required_linear_history": false,
  "allow_force_pushes": false,
  "allow_deletions": false,
  "block_creations": false,
  "required_conversation_resolution": false,
  "lock_branch": false,
  "allow_fork_syncing": false
}
'@

foreach ($branch in @("development", "pre-prod", "prod")) {
    Write-Host "Applying branch protection to '$branch'..."
    try {
        Invoke-RestMethod -Method PUT -Uri "$base/repos/$Owner/$Repo/branches/$branch/protection" `
            -Headers $headers -ContentType "application/json" -Body $protectionJson | Out-Null
    }
    catch {
        Write-Warning "Failed for '$branch': $($_.Exception.Message). Use a PAT with repo admin, or apply rules manually (CONTRIBUTING.md)."
        throw
    }
}

Write-Host "Done. Default branch: development. Protected: development, pre-prod, prod (PR required, no force-push, no delete)."
