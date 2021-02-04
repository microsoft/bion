<#
.SYNOPSIS
    Package the bion repository.
.DESCRIPTION
    Builds the bion NuGet Packages.
.PARAMETER Configuration
    The build configuration: Release or Debug. Default=Release
#>

[CmdletBinding()]
param(
    [string]
    [ValidateSet("Debug", "Release")]
    $Configuration="Release"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

$ScriptName = $([io.Path]::GetFileNameWithoutExtension($PSCommandPath))

Import-Module -Force $PSScriptRoot\NuGetUtilities.psm1

New-NuGetPackages $Configuration

Write-Information "$ScriptName SUCCEEDED."