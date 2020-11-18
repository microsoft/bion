<#
.SYNOPSIS
    Build, test, and package the bion.
.DESCRIPTION
    Builds the bion repository, runs the tests, and creates
    NuGet packages.
.PARAMETER Configuration
    The build configuration: Release or Debug. Default=Release
.PARAMETER NoRestore
    Do not restore NuGet packages.
.PARAMETER NoBuild
    Do not build.
.PARAMETER NoTest
    Do not run tests.
#>

[CmdletBinding()]
param(
    [string]
    [ValidateSet("Debug", "Release")]
    $Configuration="Release",

    [switch]
    $NoRestore,

    [switch]
    $NoBuild,

    [switch]
    $NoTest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

$ScriptName = $([io.Path]::GetFileNameWithoutExtension($PSCommandPath))

Import-Module -Force $PSScriptRoot\NuGetUtilities.psm1

$projects = Get-ChildItem -Include *.sln -Recurse

foreach ($project in $projects) {
    if (-not $NoRestore) {
        Write-Information "Restoring $project..."
        dotnet restore $project.FullName
        if ($LASTEXITCODE -ne 0) {
            Exit-WithFailureMessage $ScriptName "Build of $solutionFilePath failed."
        }
    }

    if (-not $NoBuild) {
        Write-Information "Building $project..."
        dotnet build $project.FullName --no-restore --configuration $Configuration
        if ($LASTEXITCODE -ne 0) {
            Exit-WithFailureMessage $ScriptName "Build of $solutionFilePath failed."
        }
    }

    if (-not $NoTest) {
        Write-Information "Testing $project..."
        dotnet test $project.FullName --no-build --configuration $Configuration
        if ($LASTEXITCODE -ne 0) {
            Exit-WithFailureMessage $ScriptName "Build of $solutionFilePath failed."
        }        
    }
}

Write-Information "$ScriptName SUCCEEDED."