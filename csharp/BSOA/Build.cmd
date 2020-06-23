@ECHO OFF

PUSHD "%~dp0"

ECHO.
ECHO Build BSOA
ECHO ==========
dotnet build BSOA.sln -c Release

::ECHO.
::ECHO Test BSOA
::ECHO =========
::dotnet test BSOA.sln

ECHO.
ECHO Build NuGet Packages
ECHO ====================
dotnet pack BSOA\BSOA.csproj -c Release -o "bin\NuGet" --include-symbols --no-build
dotnet pack BSOA.Json\BSOA.Json.csproj -c Release -o "bin\NuGet" --include-symbols --no-build

ECHO.
ECHO Clearing BSOA from Local NuGet Cache
ECHO ====================================
RMDIR "%USERPROFILE%\.nuget\packages\bsoa" /S /Q
RMDIR "%USERPROFILE%\.nuget\packages\bsoa.json" /S /Q

POPD



