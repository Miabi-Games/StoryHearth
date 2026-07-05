# ──────────────────────────────────────────────────────────────────────────────
# Clean-Repo.ps1
#
# Removes files that `git clean -dfxn` reports it would remove.
#
# Used as a personal workaround due to files being locked by Visual Studio when
# using `git clean -dfxn` directly. Kept in the repository for convenience and
# sharing with others who may have the same issue.
#
# This script was written by AI.
# ──────────────────────────────────────────────────────────────────────────────

Set-Location -LiteralPath $PSScriptRoot

Write-Host "Repository:" (Get-Location)
Write-Host ""

$items = git clean -dfxn |
    Where-Object { $_ -like "Would remove *" } |
    ForEach-Object { $_ -replace '^Would remove ', '' }

if (-not $items) {
    Write-Host "Nothing to clean."
    pause
    exit
}

Write-Host "The following files/folders will be removed:"
Write-Host ""

$items | ForEach-Object {
    Write-Host "  $_"
}

Write-Host ""
$answer = Read-Host "Delete these items? Type YES to continue"

if ($answer -ne "YES") {
    Write-Host "Cancelled."
    pause
    exit
}

foreach ($item in $items) {
    if (Test-Path -LiteralPath $item) {
        try {
            Remove-Item -LiteralPath $item -Recurse -Force -ErrorAction Stop
            Write-Host "Removed: $item"
        }
        catch {
            Write-Warning "Failed to remove: $item"
            Write-Warning $_.Exception.Message
        }
    }
}

Write-Host ""
Write-Host "Done."
pause
