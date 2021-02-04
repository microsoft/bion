function New-NuGetPackages($configuration) {
    dotnet pack "csharp/bsoa/BSOA.sln" --no-build --configuration $configuration --output "csharp/BSOA/bld/Nuget"
    if ($LASTEXITCODE -ne 0) {
        Exit-WithFailureMessage $ScriptName "$project NuGet package creation failed."
    }
}